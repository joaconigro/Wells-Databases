Imports Wells.CorePersistence.Repositories
Imports Wells.Base.Common

Public Class EnumFilter(Of T)
    Inherits BaseFilter(Of T)

    Property Value As Integer

    Private _EnumType As Type

    Public Overrides ReadOnly Property DisplayValue As String
        Get
            Return GetEnumDescription(_EnumType, Value)
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

    Sub New(propertyName As String, displayName As String, repo As IBussinessObjectRepository, value As Integer, enumType As Type)
        MyBase.New(propertyName, displayName, repo)
        _EnumType = enumType
        _Value = value
    End Sub

    Public Overrides Function Apply(originalList As IQueryable(Of T)) As IQueryable(Of T)
        Return From o In originalList
               Where CInt(CallByName(o, PropertyName, CallType.Get)) = _Value
               Select o
    End Function
End Class
