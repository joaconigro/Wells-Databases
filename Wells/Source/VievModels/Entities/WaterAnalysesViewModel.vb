Imports System.Reflection
Imports Wells.YPFModel
Imports Wells.ViewBase
Imports NPOI.XSSF.UserModel

Public Class WaterAnalysesViewModel
    Inherits EntitiesViewModel(Of WaterAnalysis)

    Private _SelectedEntity As WaterAnalysis
    Private _SelectedEntities As IEnumerable(Of WaterAnalysis)

    Sub New()
        MyBase.New(Nothing)
        IsNewCommandEnabled = False
        IsRemoveCommandEnabled = True
        FilterCollection = New FilterCollection(Of WaterAnalysis)
        Initialize()
        _Entities = _Repository.WaterAnalyses.All
        _ShowWellPanel = True
    End Sub

    Public Overrides ReadOnly Property FilterProperties As Dictionary(Of String, PropertyInfo)
        Get
            Return WaterAnalysis.Properties
        End Get
    End Property

    Public Overrides Property SelectedEntity As WaterAnalysis
        Get
            Return _SelectedEntity
        End Get
        Set
            SetValue(_SelectedEntity, Value)
            NotifyPropertyChanged(NameOf(WellExistsInfo))
        End Set
    End Property

    Public Overrides ReadOnly Property EditEntityCommand As ICommand = New RelayCommand(Sub()
                                                                                            'Dim vm As New EditMeasurementViewModel(SelectedEntity)
                                                                                            'If _Control.MainWindow.OpenEditEntityDialog(vm) Then
                                                                                            '    UpdateEntites()
                                                                                            'End If
                                                                                        End Sub, Function() SelectedEntity IsNot Nothing, AddressOf OnError)

    Public Overrides ReadOnly Property IsNewCommandEnabled As Boolean

    Public Overrides ReadOnly Property IsRemoveCommandEnabled As Boolean

    Public Overrides ReadOnly Property NewEntityCommand As ICommand = New RelayCommand(Sub()
                                                                                           'Dim vm As New EditMeasurementViewModel()
                                                                                           'If _Control.MainWindow.OpenEditEntityDialog(vm) Then
                                                                                           '    UpdateEntites()
                                                                                           'End If
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
                                                                                              If _Control.MainWindow.ShowMessageBox("¿Está seguro de eliminar este análisis?", "Eliminar") Then
                                                                                                  _Repository.WaterAnalyses.Remove(SelectedEntity)
                                                                                                  UpdateEntites()
                                                                                              End If
                                                                                          End Sub, Function() SelectedEntity IsNot Nothing AndAlso IsRemoveCommandEnabled, AddressOf OnError)

    ReadOnly Property OpenPiperShoellerGraphicCommand As ICommand = New RelayCommand(Sub(param)
                                                                                         If SelectedEntities IsNot Nothing AndAlso SelectedEntities.Any Then
                                                                                             Dim vm = New PiperSchoellerGraphicViewModel(SelectedEntities)
                                                                                             _MainWindow.OpenGraphicsView(vm)
                                                                                         ElseIf SelectedEntity IsNot Nothing Then
                                                                                             Dim vm = New PiperSchoellerGraphicViewModel({SelectedEntity})
                                                                                             _MainWindow.OpenGraphicsView(vm)
                                                                                         End If
                                                                                     End Sub, Function()
                                                                                                  Return (SelectedEntities IsNot Nothing AndAlso SelectedEntities.Any) OrElse SelectedEntity IsNot Nothing
                                                                                              End Function, AddressOf OnError)

    Protected Overrides Sub CreateWellFilter()
        Dim wellFilter = New WellFilter(Of WaterAnalysis)(_Repository.WaterAnalyses, False, WellType, WellProperty, SelectedWellName)
        OnCreatingFilter(wellFilter)
    End Sub

    Protected Overrides Sub SetCommandUpdates()
        MyBase.SetCommandUpdates()
        AddCommands(NameOf(SelectedEntity), {OpenPiperShoellerGraphicCommand})
        AddCommands(NameOf(SelectedEntities), {OpenPiperShoellerGraphicCommand})
    End Sub

    Private Sub UpdateEntites()
        _Entities = _Repository.WaterAnalyses.All
        NotifyPropertyChanged(NameOf(Entities))
    End Sub

    Private Async Sub ReadExcelFile(workbook As XSSFWorkbook, sheetIndex As Integer)
        Try
            ShowWaitingMessage("Leyendo análisis de agua del archivo Excel...")
            Dim water = Await Task.Run(Function() ExcelReader.ReadWaterAnalysis(workbook, sheetIndex, _progress))
            CloseWaitingMessage()

            If water.Any Then
                ShowWaitingMessage("Importando análisis...")
                Await Task.Run(Sub() _Repository.WaterAnalyses.AddRangeAsync(water))
                CloseWaitingMessage()

                ShowWaitingMessage("Guardando base de datos...")
                Await _Repository.SaveChangesAsync()

            End If
        Catch ex As Exception
            Throw ex
        Finally
            workbook.Close()
            CloseWaitingMessage()
        End Try
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

        Dim piperMenuItem As New MenuItem() With {.Header = "Piper-Schöeller", .Command = OpenPiperShoellerGraphicCommand, .CommandParameter = SelectedEntities}
        menu.Items.Add(piperMenuItem)

        If IsRemoveCommandEnabled Then
            menu.Items.Add(New Separator)
            Dim removeMenuItem As New MenuItem() With {.Header = "Eliminar", .Command = RemoveEntityCommand}
            menu.Items.Add(removeMenuItem)
        End If
        Return menu
    End Function
End Class
