Imports System.Reflection
Imports Wells.YPFModel
Imports Wells.ViewBase
Imports NPOI.XSSF.UserModel

Public Class WellsViewModel
    Inherits EntitiesViewModel(Of Well)

    Private _Entities As IQueryable(Of Well)
    Private _SelectedEntity As Well
    Private _SelectedEntities As IEnumerable(Of Well)

    Sub New()
        MyBase.New(Nothing)
        IsNewCommandEnabled = True
        IsRemoveCommandEnabled = True
        FilterCollection = New FilterCollection(Of Well)
        Initialize()
        _Entities = _Repository.Wells.All
        _ShowWellPanel = True
    End Sub

    Public Overrides ReadOnly Property Entities As IEnumerable(Of Well)
        Get
            If FilterCollection?.Any Then
                Dim l = FilterCollection.Apply(_Entities)
                _EntitiesCount = l.Count
                NotifyEntityCount()
                Return l.ToList
            End If
            _EntitiesCount = _Entities.Count
            NotifyEntityCount()
            Return _Entities.ToList
        End Get
    End Property

    Public Overrides ReadOnly Property FilterProperties As Dictionary(Of String, PropertyInfo)
        Get
            Return Well.Properties
        End Get
    End Property

    Public Overrides Property SelectedEntity As Well
        Get
            Return _SelectedEntity
        End Get
        Set
            SetValue(_SelectedEntity, Value)
            NotifyPropertyChanged(NameOf(WellExistsInfo))
        End Set
    End Property

    Public Overrides Property SelectedEntities As IEnumerable(Of Well)
        Get
            Return _SelectedEntities
        End Get
        Set
            SetValue(_SelectedEntities, Value)
        End Set
    End Property

    Public Overrides ReadOnly Property EditEntityCommand As ICommand = New RelayCommand(Sub()
                                                                                            Dim vm As New EditWellViewModel(SelectedEntity)
                                                                                            If _Control.MainWindow.OpenEditEntityDialog(vm) Then
                                                                                                UpdateEntites()
                                                                                            End If
                                                                                        End Sub, Function() SelectedEntity IsNot Nothing, AddressOf OnError)

    Public Overrides ReadOnly Property IsNewCommandEnabled As Boolean

    Public Overrides ReadOnly Property IsRemoveCommandEnabled As Boolean

    Public Overrides ReadOnly Property NewEntityCommand As ICommand = New RelayCommand(Sub()
                                                                                           Dim vm As New EditWellViewModel()
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
                                                                                              If _Control.MainWindow.ShowMessageBox("¿Está seguro de eliminar este pozo?", "Eliminar") Then
                                                                                                  _Repository.Wells.Remove(SelectedEntity)
                                                                                                  UpdateEntites()
                                                                                              End If
                                                                                          End Sub, Function() SelectedEntity IsNot Nothing AndAlso IsRemoveCommandEnabled, AddressOf OnError)

    Private Sub UpdateEntites()
        _Entities = _Repository.Wells.All
        NotifyPropertyChanged(NameOf(Entities))
    End Sub

    Private Async Sub ReadExcelFile(workbook As XSSFWorkbook, sheetIndex As Integer)
        ShowWaitingMessage("Leyendo pozos del archivo Excel...")
        Dim wells = Await Task.Run(Function() ExcelReader.ReadWells(workbook, sheetIndex, _progress))
        CloseWaitingMessage()

        If wells.Any Then
            ShowWaitingMessage("Importando pozos...")
            Await Task.Run(Sub() _Repository.Wells.AddRangeAsync(wells))
            CloseWaitingMessage()

            ShowWaitingMessage("Guardando base de datos...")
            Await _Repository.SaveChangesAsync()

        End If

        workbook.Close()
        CloseWaitingMessage()
    End Sub

    Protected Overrides Sub CreateWellFilter()
        Dim wellFilter = New WellFilter(Of Well)(_Repository.Wells, True, WellType, WellProperty, SelectedWellName)
        OnCreatingFilter(wellFilter)
    End Sub

    Public Overrides Function GetContextMenu() As ContextMenu
        Dim menu = New ContextMenu()
        Dim editMenuItem As New MenuItem() With {.Header = "Editar...", .Command = EditEntityCommand}
        Dim removeMenuItem As New MenuItem() With {.Header = "Eliminar", .Command = RemoveEntityCommand}
        menu.Items.Add(editMenuItem)
        menu.Items.Add(New Separator)
        menu.Items.Add(removeMenuItem)
        Return menu
    End Function

    Overrides ReadOnly Property WellExistsInfo As String
        Get
            If _SelectedEntity IsNot Nothing Then
                Return If(_SelectedEntity.Exists, "Pozo existente", "Pozo inexistente")
            End If
            Return String.Empty
        End Get
    End Property
End Class
