Imports Wells.CorePersistence.Repositories
Imports Wells.ViewBase

Public Class NumericFilter(Of T)
    Inherits BaseFilter(Of T)

    Property Value As Double

    Property Filter As NumericFunctions

    Sub New(propertyName As String, displayName As String, repo As IBussinessObjectRepository, value As Double, filter As NumericFunctions)
        MyBase.New(propertyName, displayName, repo)
        _Value = value
        _Filter = filter
    End Sub

    Public Overrides ReadOnly Property IsDateRangeFilter As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property DisplayValue As String
        Get
            Return Value.ToString("N2")
        End Get
    End Property

    Public Overrides Function Apply(originalList As IQueryable(Of T)) As IQueryable(Of T)
        Return From o In originalList
               Where CompareValue(CDbl(CallByName(o, PropertyName, CallType.Get)))
               Select o
    End Function

    Private Function CompareValue(objectValue As Double) As Boolean
        Select Case Filter
            Case NumericFunctions.Equal
                Return objectValue = Value
            Case NumericFunctions.Greater
                Return objectValue > Value
            Case NumericFunctions.GreaterOrEqual
                Return objectValue >= Value
            Case NumericFunctions.Lower
                Return objectValue < Value
            Case NumericFunctions.LowerOrEqual
                Return objectValue <= Value
            Case Else
                Return False
        End Select
    End Function

    Overrides ReadOnly Property Description As String
        Get
            Select Case Filter
                Case NumericFunctions.Equal
                    Return $"{DisplayPropertyName} = {DisplayValue}"
                Case NumericFunctions.Greater
                    Return $"{DisplayPropertyName} > {DisplayValue}"
                Case NumericFunctions.GreaterOrEqual
                    Return $"{DisplayPropertyName} >= {DisplayValue}"
                Case NumericFunctions.Lower
                    Return $"{DisplayPropertyName} < {DisplayValue}"
                Case NumericFunctions.LowerOrEqual
                    Return $"{DisplayPropertyName} <= {DisplayValue}"
                Case Else
                    Return DisplayPropertyName
            End Select
        End Get
    End Property
End Class

