Imports Wells.CorePersistence.Repositories

Public MustInherit Class BaseFilter(Of T)
    Implements IBaseFilter

    Protected Repo As IBussinessObjectRepository
    Private _IsEnabled As Boolean
    Protected _IsEditable As Boolean
    Property PropertyName As String Implements IBaseFilter.PropertyName
    Property DisplayPropertyName As String Implements IBaseFilter.DisplayPropertyName
    MustOverride ReadOnly Property DisplayValue As String Implements IBaseFilter.DisplayValue

    ReadOnly Property IsEditable As Boolean
        Get
            Return _IsEditable
        End Get
    End Property

    Property IsEnabled As Boolean Implements IBaseFilter.IsEnabled
        Get
            Return _IsEnabled
        End Get
        Set
            _IsEnabled = Value
            ParentCollection?.RaiseCollectionChanged()
        End Set
    End Property

    Property ParentCollection As FilterCollection(Of T)

    MustOverride ReadOnly Property IsDateRangeFilter As Boolean Implements IBaseFilter.IsDateRangeFilter

    Overridable ReadOnly Property Description As String Implements IBaseFilter.Description
        Get
            Return DisplayPropertyName
        End Get
    End Property

    Sub New(name As String, displayName As String, repo As IBussinessObjectRepository)
        _PropertyName = name
        DisplayPropertyName = displayName
        Me.Repo = repo
        IsEnabled = True
        _IsEditable = True
    End Sub

    MustOverride Function Apply(originalList As IQueryable(Of T)) As IQueryable(Of T)

End Class
