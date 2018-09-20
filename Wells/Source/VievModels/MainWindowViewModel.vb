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

    Private _Datasource As IEnumerable(Of IBusinessObject)
    Property Datasource As IEnumerable(Of IBusinessObject)
        Get
            Return _Datasource
        End Get
        Set
            _Datasource = Value
            NotifyPropertyChanged(NameOf(Datasource))
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
                                                             End Sub)

    Property OpenDatabaseCommand As ICommand = New Command(Sub()
                                                               Dim filename = _window.OpenFileDialog("Well Databases|*.mdf", "Abrir base de datos")
                                                               If Not String.IsNullOrEmpty(filename) Then
                                                                   My.Settings.DatabaseFilename = filename
                                                                   My.Settings.Save()
                                                                   OpenDatabase(filename, False)
                                                               End If
                                                           End Sub)

    Property ImportWellsFromExcelCommand As ICommand = New Command(Sub()
                                                                       Dim wb As XSSFWorkbook = Nothing
                                                                       Dim sheetIndex As Integer = -1

                                                                       If OpenExcelFile(wb, sheetIndex) Then
                                                                           ReadWellFromExcel(wb, sheetIndex)
                                                                       End If
                                                                   End Sub, Function()
                                                                                Return repo IsNot Nothing
                                                                            End Function)

    Property ImportMeasurementsFromExcelCommand As ICommand = New Command(Sub()
                                                                              Dim wb As XSSFWorkbook = Nothing
                                                                              Dim sheetIndex As Integer = -1

                                                                              If OpenExcelFile(wb, sheetIndex) Then
                                                                                  ReadMeasurementFromExcel(wb, sheetIndex)
                                                                              End If
                                                                          End Sub, Function()
                                                                                       Return repo IsNot Nothing
                                                                                   End Function)

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

            If sheetIndex = -1 Then
                Return False
            End If

            Return True
        End If
        Return False
    End Function

    Private Sub ReadWellFromExcel(workbook As XSSFWorkbook, sheetIndex As Integer)
        Dim wells = ExcelReader.ReadWells(workbook, sheetIndex)

        If wells.Any Then
            Dim rejected = repo.Wells.AddRange(wells)

            If Not rejected.Any Then
                repo.SaveChanges()
            Else
                'Mostrar reporte de rechazados
            End If
            repo.SaveChanges()
        End If

        workbook.Close()
    End Sub

    Private Sub ReadMeasurementFromExcel(workbook As XSSFWorkbook, sheetIndex As Integer)
        Dim measurements = ExcelReader.ReadMeasurements(workbook, sheetIndex)

        If measurements.Any Then
            Dim rejected = repo.Measurements.AddRange(measurements)

            If Not rejected.Any Then
                repo.SaveChanges()
            Else
                'Mostrar reporte de rechazados
            End If
            repo.SaveChanges()
        End If

        workbook.Close()
    End Sub

    Private Sub OpenDatabase(databaseFile As String, create As Boolean)
        repo?.Close()
        repo = New Repositories(databaseFile, create)
        Filter = New BaseFilter(repo) With {.ShowedDatasource = My.Settings.ShowedDatasource}
        SetDatasource()
        CType(ImportWellsFromExcelCommand, Command).RaiseCanExecuteChanged()
    End Sub

    Private Sub SetDatasource() Handles _Filter.FilterChanged
        Datasource = Filter.Apply
    End Sub

End Class

Public Enum DatasourceType
    Wells
    Measurements
    ChemicalAnalysis
    Precipitations
End Enum
