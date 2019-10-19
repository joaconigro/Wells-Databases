Imports Wells.CorePersistence.Repositories

Public Class StringFilter(Of T)
    Inherits BaseFilter(Of T)

    Property Value As String

    Public Overrides ReadOnly Property DisplayValue As String
        Get
            Return Value
        End Get
    End Property

    Public Overrides ReadOnly Property IsDateRangeFilter As Boolean
        Get
            Return False
        End Get
    End Property

    Overrides ReadOnly Property Description As String
        Get
            Return $"{DisplayPropertyName} contiene {Value}"
        End Get
    End Property

    Sub New(propertyName As String, displayName As String, repo As IBussinessObjectRepository, value As String)
        MyBase.New(propertyName, displayName, repo)
        _Value = value
    End Sub

    Public Overrides Function Apply(originalList As IQueryable(Of T)) As IQueryable(Of T)
        Return From o In originalList
               Let s = CStr(CallByName(o, PropertyName, CallType.Get))
               Where Not String.IsNullOrEmpty(s) AndAlso s.ToLower.Contains(_Value.ToLower)
               Select o
    End Function

End Class
