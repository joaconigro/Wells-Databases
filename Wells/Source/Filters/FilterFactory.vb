Imports Wells.CorePersistence.Repositories
Imports Wells.ViewBase

Public Class FilterFactory

    Shared Function CreateStringFilter(Of T)(propertyName As String, displayName As String, repo As IBussinessObjectRepository, value As String) As StringFilter(Of T)
        Return New StringFilter(Of T)(propertyName, displayName, repo, value)
    End Function

    Shared Function CreateBooleanFilter(Of T)(propertyName As String, displayName As String, repo As IBussinessObjectRepository, value As Boolean) As BooleanFilter(Of T)
        Return New BooleanFilter(Of T)(propertyName, displayName, repo, value)
    End Function

    Shared Function CreateEnumFilter(Of T)(propertyName As String, displayName As String, repo As IBussinessObjectRepository, value As Integer, enumType As Type) As EnumFilter(Of T)
        Return New EnumFilter(Of T)(propertyName, displayName, repo, value, enumType)
    End Function

    Shared Function CreateNumericFilter(Of T)(propertyName As String, displayName As String, repo As IBussinessObjectRepository, value As Double, filter As NumericFunctions) As NumericFilter(Of T)
        Return New NumericFilter(Of T)(propertyName, displayName, repo, value, filter)
    End Function

    Shared Function CreateDateRangeFilter(Of T)(propertyName As String, displayName As String, repo As IBussinessObjectRepository, startDate As Date) As DateRangeFilter(Of T)
        Return New DateRangeFilter(Of T)(propertyName, displayName, repo, startDate)
    End Function

    Shared Function CreateDateRangeFilter(Of T)(propertyName As String, displayName As String, repo As IBussinessObjectRepository, startDate As Date, endDate As Date) As DateRangeFilter(Of T)
        Return New DateRangeFilter(Of T)(propertyName, displayName, repo, startDate, endDate)
    End Function
End Class
