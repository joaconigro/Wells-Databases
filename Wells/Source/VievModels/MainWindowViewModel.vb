Imports Wells.Base
Imports Wells.Model
Imports Wells.Persistence
Imports NPOI.XSSF.UserModel

Public Class MainWindowViewModel
    Inherits BaseViewModel

    Private _window As IMainWindowView
    Private repo As Repositories

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
            Return repo?.Wells?.Names
        End Get
    End Property

    ReadOnly Property WellFilterOptions As List(Of String)
        Get
            Return {"Todos", "Pozos", "Sondeos", "Por nombre", "Zona A", "Zona B", "Zona C", "Zona D"}.ToList
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

    Private _SelectedEntity As IBusinessObject
    Property SelectedEntity As IBusinessObject
        Get
            Return _SelectedEntity
        End Get
        Set
            _SelectedEntity = Value
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
                                                                       Return repo IsNot Nothing
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
                                                                              Return repo IsNot Nothing
                                                                          End Function,
                                                                          AddressOf OnError)

    Property ShowedDatasourceCommand As ICommand = New Command(Sub(param)
                                                                   _Filter.ShowedDatasource = CInt(param)
                                                               End Sub,
                                                               Function()
                                                                   Return repo IsNot Nothing
                                                               End Function,
                                                               AddressOf OnError)

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

    Private Sub ReadWellFromExcel(workbook As XSSFWorkbook, sheetIndex As Integer)
        Dim wells = ExcelReader.ReadWells(workbook, sheetIndex)

        If wells.Any Then
            Dim rejected = repo.Wells.AddRange(wells)
            If rejected.Any Then
                ExportRejectedToExcel(rejected)
            End If
            repo.SaveChanges()
        End If

        workbook.Close()
    End Sub

    Private Sub ReadMeasurementFromExcel(workbook As XSSFWorkbook, sheetIndex As Integer)
        Dim measurements = ExcelReader.ReadMeasurements(workbook, sheetIndex)

        If measurements.Any Then
            Dim rejected = repo.Measurements.AddRange(measurements)
            If rejected.Any Then
                ExportRejectedToExcel(rejected)
            End If
            repo.SaveChanges()
        End If

        workbook.Close()
    End Sub

    Private Sub ExportRejectedToExcel(rejected As List(Of RejectedEntity))
        If _window.ShowMessageBox($"No se pudieron importar {rejected.Count} registro(s). ¿Desea exportar estos datos a un nuevo archivo Excel?", "Datos rechazados") Then
            Dim filename = _window.SaveFileDialog("Archivos de Excel|*.xlsx", "Datos rechazados")
            If Not String.IsNullOrEmpty(filename) Then
                ExcelReader.ExportRejectedToExcel(rejected, filename)
            End If
        End If
    End Sub

    Private Sub OpenDatabase(databaseFile As String, create As Boolean)
        repo?.Close()
        repo = New Repositories(databaseFile, create)
        Filter = New BaseFilter(repo) With {.ShowedDatasource = My.Settings.ShowedDatasource}
        SetDatasource()
        EventsAfterOpenDatabase()
    End Sub

    Private Sub EventsAfterOpenDatabase()
        CType(ImportWellsFromExcelCommand, Command).RaiseCanExecuteChanged()
        CType(ShowedDatasourceCommand, Command).RaiseCanExecuteChanged()
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

End Class

Public Enum DatasourceType
    Wells
    Measurements
    ChemicalAnalysis
    Precipitations
End Enum
