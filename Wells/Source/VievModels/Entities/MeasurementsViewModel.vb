Imports System.Reflection
Imports Wells.YPFModel
Imports Wells.ViewBase
Imports NPOI.XSSF.UserModel

Public Class MeasurementsViewModel
    Inherits EntitiesViewModel(Of Measurement)

    Private _SelectedEntity As Measurement
    Private _SelectedEntities As IEnumerable(Of Measurement)

    Sub New()
        MyBase.New(Nothing)
        IsNewCommandEnabled = True
        IsRemoveCommandEnabled = True
        FilterCollection = New FilterCollection(Of Measurement)
        Initialize()
        _Entities = _Repository.Measurements.All
        _ShowWellPanel = True
    End Sub

    Public Overrides ReadOnly Property FilterProperties As Dictionary(Of String, PropertyInfo)
        Get
            Return Measurement.Properties
        End Get
    End Property

    Public Overrides Property SelectedEntity As Measurement
        Get
            Return _SelectedEntity
        End Get
        Set
            SetValue(_SelectedEntity, Value)
            NotifyPropertyChanged(NameOf(WellExistsInfo))
        End Set
    End Property

    Public Overrides ReadOnly Property EditEntityCommand As ICommand = New RelayCommand(Sub()
                                                                                            Dim vm As New EditMeasurementViewModel(SelectedEntity)
                                                                                            If _Control.MainWindow.OpenEditEntityDialog(vm) Then
                                                                                                UpdateEntites()
                                                                                            End If
                                                                                        End Sub, Function() SelectedEntity IsNot Nothing, AddressOf OnError)

    Public Overrides ReadOnly Property IsNewCommandEnabled As Boolean

    Public Overrides ReadOnly Property IsRemoveCommandEnabled As Boolean

    Public Overrides ReadOnly Property NewEntityCommand As ICommand = New RelayCommand(Sub()
                                                                                           Dim vm As New EditMeasurementViewModel()
                                                                                           If _Control.MainWindow.OpenEditEntityDialog(vm) Then
                                                                                               UpdateEntites()
                                                                                           End If
                                                                                       End Sub, Function() IsNewCommandEnabled, AddressOf OnError)

    Public Overrides ReadOnly Property ImportEntitiesCommand As ICommand = New RelayCommand(Sub()
                                                                                                Dim wb As XSSFWorkbook = Nothing
                                                                                                Dim sheetIndex As Integer = -1

                                                                                                If OpenExcelFile(wb, sheetIndex) Then
                                                                                                    ReadExcelFile(wb, sheetIndex)
                                                                                                    UpdateEntites()
                                                                                                End If
                                                                                            End Sub, Function() IsNewCommandEnabled, AddressOf OnError, AddressOf CloseWaitingMessage)

    Public Overrides ReadOnly Property RemoveEntityCommand As ICommand = New RelayCommand(Sub()
                                                                                              If _Control.MainWindow.ShowMessageBox("¿Está seguro de eliminar esta medición?", "Eliminar") Then
                                                                                                  _Repository.Measurements.Remove(SelectedEntity)
                                                                                                  UpdateEntites()
                                                                                              End If
                                                                                          End Sub, Function() SelectedEntity IsNot Nothing AndAlso IsRemoveCommandEnabled, AddressOf OnError)

    Protected Overrides Sub CreateWellFilter()
        Dim wellFilter = New WellFilter(Of Measurement)(_Repository.Measurements, False, WellType, WellProperty, SelectedWellName)
        OnCreatingFilter(wellFilter)
    End Sub

    Private Sub UpdateEntites()
        _Entities = _Repository.Measurements.All
        NotifyPropertyChanged(NameOf(Entities))
    End Sub

    Private Async Sub ReadExcelFile(workbook As XSSFWorkbook, sheetIndex As Integer)
        ShowWaitingMessage("Leyendo mediciones del archivo Excel...")
        Dim measurements = Await Task.Run(Function() ExcelReader.ReadMeasurements(workbook, sheetIndex, _progress))
        CloseWaitingMessage()

        If measurements.Any Then
            ShowWaitingMessage("Importando mediciones...")
            Await Task.Run(Sub() _Repository.Measurements.AddRangeAsync(measurements))
            CloseWaitingMessage()

            ShowWaitingMessage("Guardando base de datos...")
            Await _Repository.SaveChangesAsync()

        End If
        workbook.Close()
        CloseWaitingMessage()
    End Sub

    Overrides ReadOnly Property WellExistsInfo As String
        Get
            If _SelectedEntity IsNot Nothing Then
                Return If(_SelectedEntity.Well?.Exists, "Pozo existente", "Pozo inexistente")
            End If
            Return String.Empty
        End Get
    End Property

    Public Overrides Function GetContextMenu() As ContextMenu
        Dim menu = New ContextMenu()
        Dim editMenuItem As New MenuItem() With {.Header = "Editar...", .Command = EditEntityCommand}
        Dim removeMenuItem As New MenuItem() With {.Header = "Eliminar", .Command = RemoveEntityCommand}
        menu.Items.Add(editMenuItem)
        menu.Items.Add(New Separator)
        menu.Items.Add(removeMenuItem)
        Return menu
    End Function
End Class
