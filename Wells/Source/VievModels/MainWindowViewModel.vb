Imports Wells.Model
Imports Wells.Persistence
Imports NPOI.XSSF.UserModel
Imports Wells.BaseFilter

Public Class MainWindowViewModel
    Inherits BaseViewModel

    Private _window As IMainWindowView
    Private _repo As Repositories

    Private WithEvents _Filter As BaseFilter
    Property Filter As BaseFilter
        Get
            Return _Filter
        End Get
        Set
            _Filter = Value
        End Set
    End Property

    ReadOnly Property WellNames As List(Of String)
        Get
            Return _repo?.Wells?.Names
        End Get
    End Property

    ReadOnly Property WellFilterOptions As List(Of String)
        Get
            Return {"Todos", "Pozos", "Sondeos", "Por nombre", "Zona A", "Zona B", "Zona C", "Zona D", "Zona Antorchas"}.ToList
        End Get
    End Property

    ReadOnly Property CriteriaFilterOptions As List(Of String)
        Get
            Return {"Valor exacto", "Menor que", "Menor o igual que", "Mayor que", "Mayor o igual que"}.ToList
        End Get
    End Property

    ReadOnly Property PropertiesNames As List(Of String)
        Get
            If _Filter IsNot Nothing Then
                Return _Filter.PropertiesNames
            Else
                Return New List(Of String)
            End If
        End Get
    End Property

    Private _SelectedWellFilterOption As Integer
    Property SelectedWellFilterOption As Integer
        Get
            Return _SelectedWellFilterOption
        End Get
        Set
            If _SelectedWellFilterOption <> Value Then
                _SelectedWellFilterOption = Value
                If _Filter IsNot Nothing Then
                    _Filter.SelectedWellFilterOption = _SelectedWellFilterOption
                End If
            End If
        End Set
    End Property

    Private _SelectedWellName As String
    Property SelectedWellName As String
        Get
            Return _SelectedWellName
        End Get
        Set
            If _SelectedWellName <> Value Then
                _SelectedWellName = Value
                If _Filter IsNot Nothing Then
                    _Filter.SelectedWellName = _SelectedWellName
                End If
            End If
        End Set
    End Property

    Private _WellFilter As Integer
    Property WellFilter As Integer
        Get
            Return _WellFilter
        End Get
        Set
            If _WellFilter <> Value Then
                _WellFilter = Value
                If _Filter IsNot Nothing Then
                    _Filter.WellFilter = _WellFilter
                    NotifyPropertyChanged(NameOf(WellNamesVisible))
                End If
            End If
        End Set
    End Property

    Private _StartDate As Date
    Property StartDate As Date
        Get
            Return _StartDate
        End Get
        Set
            If _StartDate <> Value Then
                _StartDate = Value
                If _Filter IsNot Nothing Then
                    _Filter.StartDate = _StartDate
                End If
            End If
        End Set
    End Property

    Private _EndDate As Date
    Property EndDate As Date
        Get
            Return _EndDate
        End Get
        Set
            If _EndDate <> Value Then
                _EndDate = Value
                If _Filter IsNot Nothing Then
                    _Filter.EndDate = _EndDate
                End If
            End If
        End Set
    End Property

    Private _PropertyName As String
    Property PropertyName As String
        Get
            Return _PropertyName
        End Get
        Set
            If _PropertyName <> Value Then
                _PropertyName = Value
                If _Filter IsNot Nothing Then
                    _Filter.PropertyName = _PropertyName
                End If
            End If
        End Set
    End Property

    Private _StringValue As String
    Property StringValue As String
        Get
            Return _doubleValue
        End Get
        Set
            If _StringValue <> Value Then
                _StringValue = Value
                If IsNumeric(Value) Then
                    _doubleValue = Double.Parse(Value, Globalization.NumberStyles.Any)
                Else
                    _StringValue = String.Empty
                End If
                If _Filter IsNot Nothing Then
                    _Filter.StringValue = _StringValue
                End If
            End If
        End Set
    End Property

    ReadOnly Property WellNamesVisible As Boolean
        Get
            If _Filter IsNot Nothing Then
                Return _Filter.WellFilter = WellQuery.ByName
            End If
            Return False
        End Get
    End Property

    Private _doubleValue As Double

    Private _parameterFilter As Integer
    Property ParameterFilter As Integer
        Get
            Return _parameterFilter
        End Get
        Set
            If _parameterFilter <> Value Then
                _parameterFilter = Value
                If _Filter IsNot Nothing Then
                    _Filter.ParameterFilter = _parameterFilter
                End If
            End If
        End Set
    End Property


    Private _SelectedEntity As IBusinessObject
    Property SelectedEntity As IBusinessObject
        Get
            Return _SelectedEntity
        End Get
        Set
            _SelectedEntity = Value
            NotifyPropertyChanged(NameOf(WellExistsInfo))
            CType(EditWellCommand, Command).RaiseCanExecuteChanged()
            CType(EditMeasurementCommand, Command).RaiseCanExecuteChanged()
        End Set
    End Property

    Private _Datasource As IEnumerable(Of IBusinessObject)
    Property Datasource As IEnumerable(Of IBusinessObject)
        Get
            Return _Datasource
        End Get
        Set
            _Datasource = Value
            NotifyPropertyChanged(NameOf(Datasource))
            NotifyPropertyChanged(NameOf(ItemsCount))
        End Set
    End Property

    Sub New(window As IMainWindowView)
        _window = window

        _progress = New Progress(Of Integer)(AddressOf ProgressChanged)

        If Not String.IsNullOrEmpty(My.Settings.DatabaseFilename) AndAlso IO.File.Exists(My.Settings.DatabaseFilename) Then
            OpenDatabase(My.Settings.DatabaseFilename, False)
        End If

    End Sub

    Property CreateDatabaseCommand As ICommand = New Command(Sub()
                                                                 Dim databaseName As String = ""
                                                                 Dim databasePath As String = ""
                                                                 If _window.CreateDatabaseDialog(databaseName, databasePath) Then
                                                                     Dim filename = IO.Path.Combine(databasePath, databaseName & ".mdf")
                                                                     My.Settings.DatabaseFilename = filename
                                                                     My.Settings.Save()
                                                                     OpenDatabase(filename, True)
                                                                 End If
                                                             End Sub, Function() True, AddressOf OnError)

    Property OpenDatabaseCommand As ICommand = New Command(Sub()
                                                               Dim filename = _window.OpenFileDialog("Well Databases|*.mdf", "Abrir base de datos")
                                                               If Not String.IsNullOrEmpty(filename) Then
                                                                   My.Settings.DatabaseFilename = filename
                                                                   My.Settings.Save()
                                                                   OpenDatabase(filename, False)
                                                               End If
                                                           End Sub, Function() True, AddressOf OnError)

    Property ImportWellsFromExcelCommand As ICommand = New Command(Sub()
                                                                       Dim wb As XSSFWorkbook = Nothing
                                                                       Dim sheetIndex As Integer = -1

                                                                       If OpenExcelFile(wb, sheetIndex) Then
                                                                           ReadWellFromExcel(wb, sheetIndex)
                                                                       End If
                                                                   End Sub,
                                                                   Function()
                                                                       Return Repositories.HasProject
                                                                   End Function,
                                                                   AddressOf OnError)

    Property ImportMeasurementsFromExcelCommand As ICommand = New Command(Sub()
                                                                              Dim wb As XSSFWorkbook = Nothing
                                                                              Dim sheetIndex As Integer = -1

                                                                              If OpenExcelFile(wb, sheetIndex) Then
                                                                                  ReadMeasurementFromExcel(wb, sheetIndex)
                                                                              End If
                                                                          End Sub,
                                                                          Function()
                                                                              Return Repositories.HasProject
                                                                          End Function,
                                                                          AddressOf OnError)

    Property ImportPrecipitationsFromExcelCommand As ICommand = New Command(Sub()
                                                                                Dim wb As XSSFWorkbook = Nothing
                                                                                Dim sheetIndex As Integer = -1

                                                                                If OpenExcelFile(wb, sheetIndex) Then
                                                                                    ReadPrecipitationsFromExcel(wb, sheetIndex)
                                                                                End If
                                                                            End Sub,
                                                                          Function()
                                                                              Return Repositories.HasProject
                                                                          End Function,
                                                                          AddressOf OnError)

    Property ShowedDatasourceCommand As ICommand = New Command(Sub(param)
                                                                   _Filter.ShowedDatasource = CInt(param)
                                                               End Sub,
                                                               Function()
                                                                   Return Repositories.HasProject
                                                               End Function,
                                                               AddressOf OnError)

    Property NewWellCommand As ICommand = New Command(Sub()
                                                          Dim vm As New EditWellViewModel
                                                          Dim result = _window.ShowEditWellDialog(vm)
                                                          If result Then
                                                              NotifyPropertyChanged(NameOf(WellNames))
                                                              SetDatasource()
                                                          End If
                                                      End Sub,
                                                      Function()
                                                          Return Repositories.HasProject
                                                      End Function, AddressOf OnError)

    Property EditWellCommand As ICommand = New Command(Sub()
                                                           Dim result As Boolean = False
                                                           If TypeOf _SelectedEntity Is Well Then
                                                               Dim vm As New EditWellViewModel(CType(_SelectedEntity, Well))
                                                               result = _window.ShowEditWellDialog(vm)
                                                           ElseIf TypeOf _SelectedEntity Is Measurement Then
                                                               Dim vm As New EditWellViewModel(CType(_SelectedEntity, Measurement).Well)
                                                               result = _window.ShowEditWellDialog(vm)
                                                           ElseIf TypeOf _SelectedEntity Is ChemicalAnalysis Then
                                                               Dim vm As New EditWellViewModel(CType(_SelectedEntity, ChemicalAnalysis).Well)
                                                               result = _window.ShowEditWellDialog(vm)
                                                           End If
                                                           If result Then
                                                               NotifyPropertyChanged(NameOf(WellNames))
                                                               SetDatasource()
                                                           End If
                                                       End Sub,
                                                      Function()
                                                          Return Repositories.HasProject AndAlso _SelectedEntity IsNot Nothing
                                                      End Function, AddressOf OnError)

    Property NewMeasurementCommand As ICommand = New Command(Sub()
                                                                 Dim vm As EditMeasurementViewModel
                                                                 If _SelectedEntity IsNot Nothing Then
                                                                     If TypeOf _SelectedEntity Is Well Then
                                                                         vm = New EditMeasurementViewModel(CType(_SelectedEntity, Well))
                                                                     Else
                                                                         vm = New EditMeasurementViewModel()
                                                                     End If
                                                                 Else
                                                                     vm = New EditMeasurementViewModel()
                                                                 End If
                                                                 Dim result = _window.ShowEditMeasurementDialog(vm)
                                                                 If result Then
                                                                     SetDatasource()
                                                                 End If
                                                             End Sub,
                                                      Function()
                                                          Return Repositories.HasProject
                                                      End Function, AddressOf OnError)

    Property EditMeasurementCommand As ICommand = New Command(Sub()
                                                                  If TypeOf _SelectedEntity Is Measurement Then
                                                                      Dim vm As New EditMeasurementViewModel(CType(_SelectedEntity, Measurement))
                                                                      Dim result = _window.ShowEditMeasurementDialog(vm)
                                                                      If result Then
                                                                          SetDatasource()
                                                                      End If
                                                                  End If
                                                              End Sub,
                                                      Function()
                                                          Return Repositories.HasProject AndAlso _SelectedEntity IsNot Nothing
                                                      End Function, AddressOf OnError)

    Private Function OpenExcelFile(ByRef workbook As XSSFWorkbook, ByRef sheetIndex As Integer) As Boolean
        Dim filename = _window.OpenFileDialog("Archivos de Excel|*.xlsx", "Importar Excel")
        If Not String.IsNullOrEmpty(filename) Then
            workbook = New XSSFWorkbook(filename)
            If workbook.NumberOfSheets > 1 Then
                Dim sheets As New List(Of String)
                For i = 0 To workbook.NumberOfSheets - 1
                    sheets.Add(workbook.GetSheetName(i))
                Next
                sheetIndex = _window.SelectSheetDialog(sheets)
            Else
                sheetIndex = 0
            End If

            If sheetIndex > -1 Then
                Return True
            End If
        End If
        Return False
    End Function

    Private Async Sub ReadWellFromExcel(workbook As XSSFWorkbook, sheetIndex As Integer)
        StartProgressNotifications(False, "Leyendo pozos del archivo Excel")
        Dim wells = Await Task.Run(Function() ExcelReader.ReadWells(workbook, sheetIndex, _progress))

        If wells.Any Then
            StartProgressNotifications(False, "Importando pozos")
            Dim rejected = Await Task.Run(Function() _repo.Wells.AddRange(wells, _progress))
            StartProgressNotifications(True, "Guardando base de datos")
            Await _repo.SaveChangesAsync()
            If rejected.Any Then
                ExportRejectedToExcel(rejected)
            End If
        End If

        workbook.Close()
        StopProgressNotifications()
        SetDatasource()
    End Sub

    Private Async Sub ReadMeasurementFromExcel(workbook As XSSFWorkbook, sheetIndex As Integer)
        StartProgressNotifications(False, "Leyendo mediciones del archivo Excel")
        Dim measurements = Await Task.Run(Function() ExcelReader.ReadMeasurements(workbook, sheetIndex, _progress))

        If measurements.Any Then
            StartProgressNotifications(False, "Importando mediciones")
            Dim rejected = Await Task.Run(Function() _repo.Measurements.AddRange(measurements, _progress))
            StartProgressNotifications(True, "Guardando base de datos")
            Await _repo.SaveChangesAsync()
            If rejected.Any Then
                ExportRejectedToExcel(rejected)
            End If
        End If

        workbook.Close()
        StopProgressNotifications()
        SetDatasource()
    End Sub

    Private Async Sub ReadPrecipitationsFromExcel(workbook As XSSFWorkbook, sheetIndex As Integer)
        StartProgressNotifications(False, "Leyendo precipitaciones del archivo Excel")
        Dim precipitations = Await Task.Run(Function() ExcelReader.ReadPrecipitations(workbook, sheetIndex, _progress))

        If precipitations.Any Then
            StartProgressNotifications(False, "Importando precipitaciones")
            Dim rejected = Await Task.Run(Function() _repo.Precipitations.AddRange(precipitations, _progress))
            StartProgressNotifications(True, "Guardando base de datos")
            Await _repo.SaveChangesAsync()
            'If rejected.Any Then
            '    ExportRejectedToExcel(rejected)
            'End If
        End If

        workbook.Close()
        StopProgressNotifications()
        SetDatasource()
    End Sub

    Private Sub ExportRejectedToExcel(rejected As List(Of RejectedEntity))
        If _window.ShowMessageBox($"No se pudieron importar {rejected.Count} registro(s). ¿Desea exportar estos datos a un nuevo archivo Excel?", "Datos rechazados") Then
            Dim filename = _window.SaveFileDialog("Archivos de Excel|*.xlsx", "Datos rechazados")
            If Not String.IsNullOrEmpty(filename) Then
                ExcelReader.ExportRejectedToExcel(rejected, filename)
            End If
        End If
    End Sub

    Private Async Sub OpenDatabase(databaseFile As String, create As Boolean)
        _repo?.Close()
        StartProgressNotifications(True, "Abriendo la base de datos")
        Await Task.Run(Sub() Repositories.CreateOrOpen(databaseFile, create))
        _repo = Repositories.Instance
        If _repo IsNot Nothing Then
            Filter = New BaseFilter()
            SetDatasource()
        End If
        EventsAfterOpenDatabase()
    End Sub

    Private Sub EventsAfterOpenDatabase()
        CType(ImportWellsFromExcelCommand, Command).RaiseCanExecuteChanged()
        CType(ImportMeasurementsFromExcelCommand, Command).RaiseCanExecuteChanged()
        CType(ImportPrecipitationsFromExcelCommand, Command).RaiseCanExecuteChanged()
        CType(ShowedDatasourceCommand, Command).RaiseCanExecuteChanged()
        CType(NewWellCommand, Command).RaiseCanExecuteChanged()
        CType(NewMeasurementCommand, Command).RaiseCanExecuteChanged()
        NotifyPropertyChanged(NameOf(WellNames))
        NotifyPropertyChanged(NameOf(PropertiesNames))
        StopProgressNotifications()
    End Sub

    Private Sub SetDatasource() Handles _Filter.FilterChanged
        Datasource = Filter.Apply
    End Sub

    Private Sub OnFilterDatasourceTypeChanged() Handles _Filter.DatasoureceTypeChanged
        NotifyPropertyChanged(NameOf(PropertiesNames))
    End Sub

    Protected Overrides Sub ShowErrorMessage(message As String)
        _window.ShowErrorMessageBox(message)
    End Sub

    ReadOnly Property ItemsCount As Integer
        Get
            Return If(_Datasource IsNot Nothing, Datasource.Count, 0)
        End Get
    End Property

    ReadOnly Property WellExistsInfo As String
        Get
            If _SelectedEntity IsNot Nothing Then
                If TypeOf _SelectedEntity Is Well Then
                    Return If(CType(_SelectedEntity, Well).Exists, "Pozo existente", "Pozo inexistente")
                ElseIf TypeOf _SelectedEntity Is Measurement Then
                    Return If(CType(_SelectedEntity, Measurement).Well.Exists, "Pozo existente", "Pozo inexistente")
                ElseIf TypeOf _SelectedEntity Is ChemicalAnalysis Then
                    Return If(CType(_SelectedEntity, ChemicalAnalysis).Well.Exists, "Pozo existente", "Pozo inexistente")
                End If
            End If
            Return String.Empty
        End Get
    End Property

End Class

Public Enum DatasourceType
    Wells
    Measurements
    ChemicalAnalysis
    Precipitations
End Enum
