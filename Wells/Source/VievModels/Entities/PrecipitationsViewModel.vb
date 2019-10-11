Imports System.Reflection
Imports Wells.YPFModel
Imports Wells.ViewBase
Imports NPOI.XSSF.UserModel

Public Class PrecipitationsViewModel
    Inherits EntitiesViewModel(Of Precipitation)

    Private _SelectedEntity As Precipitation
    Private _SelectedEntities As IEnumerable(Of Precipitation)

    Sub New()
        MyBase.New(Nothing)
        IsNewCommandEnabled = False
        IsRemoveCommandEnabled = True
        FilterCollection = New FilterCollection(Of Precipitation)
        Initialize()
        _Entities = _Repository.Precipitations.All
    End Sub

    Public Overrides ReadOnly Property FilterProperties As Dictionary(Of String, PropertyInfo)
        Get
            Return Precipitation.Properties
        End Get
    End Property

    Public Overrides Property SelectedEntity As Precipitation
        Get
            Return _SelectedEntity
        End Get
        Set
            SetValue(_SelectedEntity, Value)
        End Set
    End Property

    Public Overrides ReadOnly Property EditEntityCommand As ICommand = New RelayCommand(Sub()

                                                                                        End Sub, Function() SelectedEntity IsNot Nothing, AddressOf OnError)

    Public Overrides ReadOnly Property IsNewCommandEnabled As Boolean

    Public Overrides ReadOnly Property IsRemoveCommandEnabled As Boolean

    Public Overrides ReadOnly Property NewEntityCommand As ICommand = New RelayCommand(Sub()

                                                                                       End Sub, Function() IsNewCommandEnabled, AddressOf OnError)

    Public Overrides ReadOnly Property ImportEntitiesCommand As ICommand = New RelayCommand(Sub()
                                                                                                Dim wb As XSSFWorkbook = Nothing
                                                                                                Dim sheetIndex As Integer = -1

                                                                                                If OpenExcelFile(wb, sheetIndex) Then
                                                                                                    ReadExcelFile(wb, sheetIndex)
                                                                                                    UpdateEntites()
                                                                                                End If
                                                                                            End Sub, Function() True, AddressOf OnError, AddressOf CloseWaitingMessage)

    Public Overrides ReadOnly Property RemoveEntityCommand As ICommand = New RelayCommand(Sub()
                                                                                              If _Control.MainWindow.ShowMessageBox("¿Está seguro de eliminar este dato de precipitación?", "Eliminar") Then
                                                                                                  _Repository.Precipitations.Remove(SelectedEntity)
                                                                                                  UpdateEntites()
                                                                                              End If
                                                                                          End Sub, Function() SelectedEntity IsNot Nothing AndAlso IsRemoveCommandEnabled, AddressOf OnError)

    Protected Overrides Sub CreateWellFilter()

    End Sub

    Private Sub UpdateEntites()
        _Entities = _Repository.Precipitations.All
        NotifyPropertyChanged(NameOf(Entities))
    End Sub

    Private Async Sub ReadExcelFile(workbook As XSSFWorkbook, sheetIndex As Integer)
        ShowWaitingMessage("Leyendo precipitaciones del archivo Excel...")
        Dim precipitations = Await Task.Run(Function() ExcelReader.ReadPrecipitations(workbook, sheetIndex, _progress))
        CloseWaitingMessage()

        If precipitations.Any Then
            ShowWaitingMessage("Importando análisis...")
            Await Task.Run(Sub() _Repository.Precipitations.AddRangeAsync(precipitations))
            CloseWaitingMessage()

            ShowWaitingMessage("Guardando base de datos...")
            Await _Repository.SaveChangesAsync()

        End If
        workbook.Close()
        CloseWaitingMessage()
    End Sub

    Public Overrides Function GetContextMenu() As ContextMenu
        Return Nothing
    End Function
End Class
