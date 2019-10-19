Imports System.Reflection
Imports Wells.YPFModel
Imports Wells.ViewBase
Imports NPOI.XSSF.UserModel

Public Class FLNAAnalysesViewModel
    Inherits EntitiesViewModel(Of FLNAAnalysis)

    Private _SelectedEntity As FLNAAnalysis
    Private _SelectedEntities As IEnumerable(Of FLNAAnalysis)

    Sub New()
        MyBase.New(Nothing)
        IsNewCommandEnabled = False
        IsRemoveCommandEnabled = True
        FilterCollection = New FilterCollection(Of FLNAAnalysis)
        Initialize()
        _Entities = _Repository.FLNAAnalyses.All
        _ShowWellPanel = True
    End Sub

    Public Overrides ReadOnly Property FilterProperties As Dictionary(Of String, PropertyInfo)
        Get
            Return FLNAAnalysis.Properties
        End Get
    End Property

    Public Overrides Property SelectedEntity As FLNAAnalysis
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
                                                                                                  _Repository.FLNAAnalyses.Remove(SelectedEntity)
                                                                                                  UpdateEntites()
                                                                                              End If
                                                                                          End Sub, Function() SelectedEntity IsNot Nothing AndAlso IsRemoveCommandEnabled, AddressOf OnError)

    Protected Overrides Sub CreateWellFilter()
        Dim wellFilter = New WellFilter(Of FLNAAnalysis)(_Repository.FLNAAnalyses, False, WellType, WellProperty, SelectedWellName)
        OnCreatingFilter(wellFilter)
    End Sub

    Private Sub UpdateEntites()
        _Entities = _Repository.FLNAAnalyses.All
        NotifyPropertyChanged(NameOf(Entities))
    End Sub

    Private Async Sub ReadExcelFile(workbook As XSSFWorkbook, sheetIndex As Integer)
        ShowWaitingMessage("Leyendo análisis de FLNA del archivo Excel...")
        Dim flna = Await Task.Run(Function() ExcelReader.ReadFLNAAnalysis(workbook, sheetIndex, _progress))
        CloseWaitingMessage()

        If flna.Any Then
            ShowWaitingMessage("Importando análisis...")
            Await Task.Run(Sub() _Repository.FLNAAnalyses.AddRangeAsync(flna))
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
        Return Nothing
    End Function
End Class
