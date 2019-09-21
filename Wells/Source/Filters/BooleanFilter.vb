Imports Wells.CorePersistence.Repositories

Public Class BooleanFilter(Of T)
    Inherits BaseFilter(Of T)

    Property Value As Boolean

    Public Overrides ReadOnly Property DisplayValue As String
        Get
            Return If(Value, "Verdadero", "Falso")
        End Get
    End Property

    Public Overrides ReadOnly Property IsDateRangeFilter As Boolean
        Get
            Return False
        End Get
    End Property

    Overrides ReadOnly Property Description As String
        Get
            Return $"{DisplayPropertyName} = {DisplayValue}"
        End Get
    End Property

    Sub New(propertyName As String, displayName As String, repo As IBussinessObjectRepository, value As Boolean)
        MyBase.New(propertyName, displayName, repo)
        _Value = value
    End Sub

    Public Overrides Function Apply(originalList As IQueryable(Of T)) As IQueryable(Of T)
        Return From o In originalList
               Where CBool(CallByName(o, PropertyName, CallType.Get)) = _Value
               Select o
    End Function
End Class
