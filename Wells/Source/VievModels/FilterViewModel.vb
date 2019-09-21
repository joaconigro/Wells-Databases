Imports System.Reflection
Imports Wells.ViewBase
Imports Wells.Base.Common
Public Class FilterViewModel
    Inherits BaseViewModel

    Private _Properties As Dictionary(Of String, PropertyInfo)
    Private _SelectedPropertyIndex As Integer

    Property ShowStringPanel As Boolean
    Property ShowNumericPanel As Boolean
    Property ShowBooleanPanel As Boolean
    Property ShowDatePanel As Boolean
    Property ShowEnumPanel As Boolean
    Property NumericValue As Double
    Property StringValue As String
    Property SelectedEnumValue As Integer
    Property StartDate As Date
    Property EndDate As Date
    Property BooleanValue As Boolean
    Property FilterType As Type
    Property SelectedMathFunction As Integer
    Property PropertyName As String
    Property PropertyDisplayName As String
    Property IsIntegerNumericType As Boolean
    Property IsCreatingFilter As Boolean
    Property DialogTitle As String

    Sub New(properties As Dictionary(Of String, PropertyInfo))
        MyBase.New(Nothing)
        _Properties = properties
        EndDate = Date.Today
        BooleanValue = True
        SelectedEnumValue = 0
        IsIntegerNumericType = False
        IsCreatingFilter = True
        If properties.Any Then
            SelectedPropertyIndex = 0
        End If
        DialogTitle = "Crear filtro"
    End Sub

    Shared Function CreateInstance(Of T)(filter As BaseFilter(Of T), properties As Dictionary(Of String, PropertyInfo)) As FilterViewModel
        Dim vm As New FilterViewModel(properties)
        vm.SelectedPropertyIndex = vm.PropertiesNames.IndexOf(filter.DisplayPropertyName)
        vm.IsCreatingFilter = False
        vm.DialogTitle = "Editar filtro"
        Return vm
    End Function

    Protected Overrides Sub SetValidators()

    End Sub

    Protected Overrides Sub SetCommandUpdates()

    End Sub

    ReadOnly Property PropertiesNames As List(Of String)
        Get
            Dim list = _Properties.Keys.ToList
            list.Sort()
            Return list
        End Get
    End Property

    ReadOnly Property EnumValues As List(Of String)
        Get
            If FilterType.IsEnum Then
                Return EnumDescriptionsToList(FilterType)
            End If
            Return Nothing
        End Get
    End Property

    ReadOnly Property MathFunctionsNames As List(Of String)
        Get
            Return EnumDescriptionsToList(GetType(NumericFunctions))
        End Get
    End Property

    Property SelectedPropertyIndex As Integer
        Get
            Return _SelectedPropertyIndex
        End Get
        Set
            _SelectedPropertyIndex = Value
            OnPropertySelected()
            NotifyPropertyChanged(NameOf(SelectedPropertyIndex))
        End Set
    End Property

    Private Sub OnPropertySelected()
        PropertyDisplayName = PropertiesNames(SelectedPropertyIndex)
        PropertyName = _Properties(PropertyDisplayName).Name
        FilterType = _Properties(PropertyDisplayName).PropertyType
        If IsNumericType(FilterType) Then
            ShowNumericPanel = True
            ShowStringPanel = False
            ShowDatePanel = False
            ShowBooleanPanel = False
            ShowEnumPanel = False
            IsIntegerNumericType = Base.IsIntegerNumericType(FilterType)
        Else
            If FilterType.IsEnum Then
                ShowNumericPanel = False
                ShowStringPanel = False
                ShowDatePanel = False
                ShowBooleanPanel = False
                ShowEnumPanel = True
                NotifyPropertyChanged(NameOf(EnumValues))
            ElseIf FilterType Is GetType(String) Then
                ShowNumericPanel = False
                ShowStringPanel = True
                ShowDatePanel = False
                ShowBooleanPanel = False
                ShowEnumPanel = False
            ElseIf FilterType Is GetType(Date) Then
                ShowNumericPanel = False
                ShowStringPanel = False
                ShowDatePanel = True
                ShowEnumPanel = False
                ShowBooleanPanel = False
            ElseIf FilterType Is GetType(Boolean) Then
                ShowNumericPanel = False
                ShowStringPanel = False
                ShowDatePanel = False
                ShowEnumPanel = False
                ShowBooleanPanel = True
            Else
                View.ShowErrorMessageBox("No se puede filtrar por esta propiedad.")
            End If
        End If

        NotifyPropertyChanged(NameOf(ShowDatePanel))
        NotifyPropertyChanged(NameOf(ShowStringPanel))
        NotifyPropertyChanged(NameOf(ShowNumericPanel))
        NotifyPropertyChanged(NameOf(ShowBooleanPanel))
        NotifyPropertyChanged(NameOf(ShowEnumPanel))
    End Sub

End Class
