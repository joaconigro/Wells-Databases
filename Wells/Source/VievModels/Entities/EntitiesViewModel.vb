Imports Wells.ViewBase
Imports Wells.CorePersistence.Repositories
Imports System.Reflection
Imports Wells.Base.Common
Imports NPOI.XSSF.UserModel
Imports Wells

Public MustInherit Class EntitiesViewModel(Of T)
    Inherits BaseViewModel
    Implements IEntitiesViewModel

    Protected _Repository As RepositoryWrapper

    Protected _Control As IEntitiesControl
    Protected _MainWindow As IMainWindowView
    Protected WithEvents _FilterCollection As FilterCollection(Of T)
    Private _SelectedFilter As BaseFilter(Of T)
    Protected _EntitiesCount As Integer
    Protected _ShowWellPanel As Boolean
    Private _WellType As Integer
    Private _WellProperty As Integer
    Private _SelectedWellName As String
    Private _SelectedEntities As IEnumerable(Of T)
    Protected _Entities As IQueryable(Of T)

    ReadOnly Property EntitiesCount As Integer
        Get
            Return _EntitiesCount
        End Get
    End Property

    Public ReadOnly Property Entities As IEnumerable(Of T)
        Get
            If FilterCollection?.Any Then
                Dim list = FilterCollection.Apply(_Entities)
                _EntitiesCount = list.Count
                NotifyEntityCount()
                Return list.ToList
            End If
            _EntitiesCount = _Entities.Count
            NotifyEntityCount()
            Return _Entities.ToList
        End Get
    End Property

    Protected Sub NotifyEntityCount()
        NotifyPropertyChanged(NameOf(EntitiesCount))
    End Sub

    Property FilterCollection As FilterCollection(Of T)
        Get
            Return _FilterCollection
        End Get
        Set
            _FilterCollection = Value
        End Set
    End Property

    Sub New(view As IView)
        MyBase.New(view)
        _Repository = RepositoryWrapper.Instance
    End Sub

    Protected Overrides Sub OnSetView(view As IView)
        _Control = CType(Me.View, IEntitiesControl)
        _MainWindow = _Control.MainWindow
    End Sub

    Protected Overrides Sub SetCommandUpdates()
        AddCommands(NameOf(SelectedEntity), {EditEntityCommand, RemoveEntityCommand})
        AddCommands(NameOf(SelectedFilter), {EditFilterCommand, RemoveFilterCommand})
        AddCommands(NameOf(WellType), {AddWellFilterCommand})
        AddCommands(NameOf(WellProperty), {AddWellFilterCommand})
        AddCommands(NameOf(SelectedWellName), {AddWellFilterCommand})
    End Sub

    Protected Overrides Sub SetValidators()

    End Sub

    Property SelectedFilter As BaseFilter(Of T)
        Get
            Return _SelectedFilter
        End Get
        Set
            SetValue(_SelectedFilter, Value)
        End Set
    End Property

    MustOverride ReadOnly Property FilterProperties As Dictionary(Of String, PropertyInfo)

    Public ReadOnly Property RemoveFilterCommand As ICommand = New RelayCommand(Sub()
                                                                                    FilterCollection.Remove(SelectedFilter)
                                                                                    OnFiltersChanged()
                                                                                End Sub, Function() SelectedFilter IsNot Nothing, AddressOf OnError)

    Public ReadOnly Property EditFilterCommand As ICommand = New RelayCommand(Sub()
                                                                                  Dim vm = FilterViewModel.CreateInstance(Of T)(SelectedFilter, FilterProperties)
                                                                                  SetDataToFilterViewModel(vm)
                                                                                  If _Control.OpenFilterDialog(vm) Then
                                                                                      EditFilter(vm)
                                                                                      _Control.ForceListBoxItemsRefresh()
                                                                                      OnFiltersChanged()
                                                                                  End If
                                                                              End Sub, Function() SelectedFilter IsNot Nothing AndAlso SelectedFilter.IsEditable, AddressOf OnError)

    Public ReadOnly Property AddFilterCommand As ICommand = New RelayCommand(Sub()
                                                                                 Dim vm As New FilterViewModel(FilterProperties)
                                                                                 If _Control.OpenFilterDialog(vm) Then
                                                                                     CreateFilter(vm)
                                                                                     OnFiltersChanged()
                                                                                 End If
                                                                             End Sub, Function() True, AddressOf OnError)

    Public ReadOnly Property AddWellFilterCommand As ICommand = New RelayCommand(Sub()
                                                                                     CreateWellFilter()
                                                                                     OnFiltersChanged()
                                                                                 End Sub, Function()
                                                                                              If WellType > 0 OrElse WellProperty > 0 Then
                                                                                                  If WellProperty <> 1 Then
                                                                                                      Return True
                                                                                                  End If
                                                                                                  If WellProperty = 1 AndAlso Not String.IsNullOrEmpty(SelectedWellName) Then
                                                                                                      Return True
                                                                                                  End If
                                                                                                  Return False
                                                                                              End If
                                                                                              Return False
                                                                                          End Function, AddressOf OnError)

    Public MustOverride Property SelectedEntity As T

    Public Property SelectedEntities As IEnumerable(Of T)
        Get
            Return _SelectedEntities
        End Get
        Set
            SetValue(_SelectedEntities, Value)
        End Set
    End Property
    Public MustOverride ReadOnly Property EditEntityCommand As ICommand Implements IEntitiesViewModel.EditEntityCommand
    Public MustOverride ReadOnly Property IsNewCommandEnabled As Boolean Implements IEntitiesViewModel.IsNewCommandEnabled
    Public MustOverride ReadOnly Property IsRemoveCommandEnabled As Boolean Implements IEntitiesViewModel.IsRemoveCommandEnabled
    Public MustOverride ReadOnly Property NewEntityCommand As ICommand Implements IEntitiesViewModel.NewEntityCommand
    Public MustOverride ReadOnly Property ImportEntitiesCommand As ICommand Implements IEntitiesViewModel.ImportEntitiesCommand
    Public MustOverride ReadOnly Property RemoveEntityCommand As ICommand Implements IEntitiesViewModel.RemoveEntityCommand

    Protected MustOverride Sub CreateWellFilter()
    Private Sub CreateFilter(vm As FilterViewModel)
        Dim repo = _Repository.Repository(Of T)()
        Dim f As BaseFilter(Of T) = Nothing

        If vm.ShowNumericPanel Then
            f = FilterFactory.CreateNumericFilter(Of T)(vm.PropertyName, vm.PropertyDisplayName, repo, vm.NumericValue, vm.SelectedMathFunction)
        ElseIf vm.ShowStringPanel Then
            f = FilterFactory.CreateStringFilter(Of T)(vm.PropertyName, vm.PropertyDisplayName, repo, vm.StringValue)
        ElseIf vm.ShowDatePanel Then
            f = FilterFactory.CreateDateRangeFilter(Of T)(vm.PropertyName, vm.PropertyDisplayName, repo, vm.StartDate, vm.EndDate)
        ElseIf vm.ShowBooleanPanel Then
            f = FilterFactory.CreateBooleanFilter(Of T)(vm.PropertyName, vm.PropertyDisplayName, repo, vm.BooleanValue)
        ElseIf vm.ShowEnumPanel Then
            f = FilterFactory.CreateEnumFilter(Of T)(vm.PropertyName, vm.PropertyDisplayName, repo, vm.SelectedEnumValue, vm.FilterType)
        End If

        OnCreatingFilter(f)
    End Sub

    Protected Sub OnCreatingFilter(f As BaseFilter(Of T))
        If f IsNot Nothing Then
            FilterCollection.Add(f)
            f.ParentCollection = FilterCollection
        End If
    End Sub

    Private Sub EditFilter(vm As FilterViewModel)
        If vm.ShowNumericPanel Then
            CType(SelectedFilter, NumericFilter(Of T)).Value = vm.NumericValue
            CType(SelectedFilter, NumericFilter(Of T)).Filter = vm.SelectedMathFunction
        ElseIf vm.ShowStringPanel Then
            CType(SelectedFilter, StringFilter(Of T)).Value = vm.StringValue
        ElseIf vm.ShowDatePanel Then
            CType(SelectedFilter, DateRangeFilter(Of T)).StartDate = vm.StartDate
            CType(SelectedFilter, DateRangeFilter(Of T)).EndDate = vm.EndDate
        ElseIf vm.ShowBooleanPanel Then
            CType(SelectedFilter, BooleanFilter(Of T)).Value = vm.BooleanValue
        ElseIf vm.ShowEnumPanel Then
            CType(SelectedFilter, EnumFilter(Of T)).Value = vm.SelectedEnumValue
        End If
    End Sub

    Private Sub SetDataToFilterViewModel(vm As FilterViewModel)
        If vm.ShowNumericPanel Then
            vm.NumericValue = CType(SelectedFilter, NumericFilter(Of T)).Value
            vm.SelectedMathFunction = CType(SelectedFilter, NumericFilter(Of T)).Filter
        ElseIf vm.ShowStringPanel Then
            vm.StringValue = CType(SelectedFilter, StringFilter(Of T)).Value
        ElseIf vm.ShowDatePanel Then
            vm.StartDate = CType(SelectedFilter, DateRangeFilter(Of T)).StartDate
            vm.EndDate = CType(SelectedFilter, DateRangeFilter(Of T)).EndDate
        ElseIf vm.ShowBooleanPanel Then
            vm.BooleanValue = CType(SelectedFilter, BooleanFilter(Of T)).Value
        ElseIf vm.ShowEnumPanel Then
            vm.SelectedEnumValue = CType(SelectedFilter, EnumFilter(Of T)).Value
        End If
    End Sub

    Protected Sub OnFiltersChanged() Handles _FilterCollection.FiltersChanged
        NotifyPropertyChanged(NameOf(Entities))
        NotifyPropertyChanged(NameOf(FilterCollection))
    End Sub

    ReadOnly Property ShowWellPanel As Boolean
        Get
            Return _ShowWellPanel
        End Get
    End Property
    ReadOnly Property WellNames As List(Of String)
        Get
            Return _Repository.Wells.Names
        End Get
    End Property

    ReadOnly Property WellTypes As List(Of String)
        Get
            Return EnumDescriptionsToList(GetType(WellTypes))
        End Get
    End Property

    ReadOnly Property WellProperties As List(Of String)
        Get
            Return EnumDescriptionsToList(GetType(WellQueryProperty))
        End Get
    End Property

    Property WellType As Integer
        Get
            Return _WellType
        End Get
        Set
            SetValue(_WellType, Value)
        End Set
    End Property

    Property WellProperty As Integer
        Get
            Return _WellProperty
        End Get
        Set
            SetValue(_WellProperty, Value)
            NotifyPropertyChanged(NameOf(WellNamesVisible))
        End Set
    End Property

    Property SelectedWellName As String
        Get
            Return _SelectedWellName
        End Get
        Set
            SetValue(_SelectedWellName, Value)
        End Set
    End Property

    ReadOnly Property WellNamesVisible As Boolean
        Get
            Return _WellProperty = WellQueryProperty.Name
        End Get
    End Property

    Overridable ReadOnly Property WellExistsInfo As String
        Get
            Return String.Empty
        End Get
    End Property

    Protected Sub ShowWaitingMessage(message As String)
        _Control.MainWindow.ShowWaitingMessage(message)
    End Sub

    Protected Sub CloseWaitingMessage()
        _Control.MainWindow.CloseWaitingMessage()
    End Sub

    Protected Function OpenExcelFile(ByRef workbook As XSSFWorkbook, ByRef sheetIndex As Integer) As Boolean
        Dim filename = _Control.MainWindow.OpenFileDialog("Archivos de Excel|*.xlsx", "Importar Excel")
        If Not String.IsNullOrEmpty(filename) Then
            workbook = New XSSFWorkbook(filename)
            If workbook.NumberOfSheets > 1 Then
                Dim sheets As New List(Of String)
                For i = 0 To workbook.NumberOfSheets - 1
                    sheets.Add(workbook.GetSheetName(i))
                Next
                sheetIndex = _Control.MainWindow.SelectSheetDialog(sheets)
            Else
                sheetIndex = 0
            End If

            If sheetIndex > -1 Then
                Return True
            End If
        End If
        Return False
    End Function

    Public MustOverride Function GetContextMenu() As ContextMenu Implements IEntitiesViewModel.GetContextMenu

    Public Sub SetSelectedEntities(entities As IEnumerable(Of Object)) Implements IEntitiesViewModel.SetSelectedEntities
        Dim tEntities = entities.Select(Function(o) CType(o, T)).ToList
        SelectedEntities = tEntities
    End Sub
End Class
