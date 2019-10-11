Imports System.Reflection
Imports Wells.YPFModel
Imports Wells.ViewBase
Imports NPOI.XSSF.UserModel
Imports System.IO
Imports System.Xml.Serialization

Public Class WellsViewModel
    Inherits EntitiesViewModel(Of Well)

    Private _SelectedEntity As Well
    Private _SelectedEntities As IEnumerable(Of Well)
    Private _PremadeGraphics As List(Of PremadeSeriesInfoCollection)

    Sub New()
        MyBase.New(Nothing)
        IsNewCommandEnabled = True
        IsRemoveCommandEnabled = True
        FilterCollection = New FilterCollection(Of Well)
        Initialize()
        _Entities = _Repository.Wells.All
        _ShowWellPanel = True
        _PremadeGraphics = ReadPremadeGraphics()
    End Sub

    Protected Overrides Sub OnSetView(view As IView)
        MyBase.OnSetView(view)
        AddHandler _Control.MainWindow.PremadeGraphicsChanged, AddressOf OnPremadeGraphicsChanged
    End Sub

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

    ReadOnly Property OpenPremadeGraphicCommand As ICommand = New RelayCommand(Sub(param)
                                                                                   Dim pg = CType(param, PremadeSeriesInfoCollection)
                                                                                   _Control.MainWindow.OpenGraphicsView(SelectedEntity, pg)
                                                                               End Sub, Function() True, AddressOf OnError)

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

        Dim graphicsMenuItem As New MenuItem() With {.Header = "Gráficos"}
        For Each pg In _PremadeGraphics
            Dim aMenuItem As New MenuItem() With {.Header = pg.Title, .Command = OpenPremadeGraphicCommand, .CommandParameter = pg}
            graphicsMenuItem.Items.Add(aMenuItem)
        Next
        menu.Items.Add(graphicsMenuItem)

        menu.Items.Add(New Separator)
        menu.Items.Add(removeMenuItem)
        Return menu
    End Function

    Private Function ReadPremadeGraphics() As List(Of PremadeSeriesInfoCollection)
        Dim filename = Path.Combine(Directory.GetCurrentDirectory, "PremadeGraphics.wpg")

        If File.Exists(filename) Then
            Dim serializer As New XmlSerializer(GetType(List(Of PremadeSeriesInfoCollection)))
            Dim entities As List(Of PremadeSeriesInfoCollection) = Nothing
            Using reader As New IO.StreamReader(filename)
                entities = CType(serializer.Deserialize(reader), List(Of PremadeSeriesInfoCollection))
            End Using

            Return entities
        End If
        Return Nothing
    End Function

    Private Sub OnPremadeGraphicsChanged()
        _PremadeGraphics = ReadPremadeGraphics()
        _Control.UpdateRowContextMenu()
    End Sub

    Overrides ReadOnly Property WellExistsInfo As String
        Get
            If _SelectedEntity IsNot Nothing Then
                Return If(_SelectedEntity.Exists, "Pozo existente", "Pozo inexistente")
            End If
            Return String.Empty
        End Get
    End Property
End Class
