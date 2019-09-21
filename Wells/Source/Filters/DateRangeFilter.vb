Imports Wells.CorePersistence.Repositories

Public Class DateRangeFilter(Of T)
    Inherits BaseFilter(Of T)

    Property StartDate As Date

    Property EndDate As Date

    Public Overrides ReadOnly Property DisplayValue As String
        Get
            Return $"Entre {StartDate.ToString("dd/MM/YY")} y {EndDate.ToString("dd/MM/YY")}"
        End Get
    End Property

    Public Overrides ReadOnly Property IsDateRangeFilter As Boolean
        Get
            Return True
        End Get
    End Property

    Sub New(propertyName As String, displayName As String, repo As IBussinessObjectRepository, startDate As Date)
        MyBase.New(propertyName, displayName, repo)
        Me.StartDate = startDate
        EndDate = Date.Today
    End Sub

    Sub New(propertyName As String, displayName As String, repo As IBussinessObjectRepository, startDate As Date, endDate As Date)
        Me.New(propertyName, displayName, repo, startDate)
        Me.EndDate = endDate
    End Sub

    Public Overrides Function Apply(originalList As IQueryable(Of T)) As IQueryable(Of T)
        Return From o In originalList
               Where CompareValue(CType(CallByName(o, PropertyName, CallType.Get), Date))
               Select o
    End Function

    Private Function CompareValue(objectValue As Date) As Boolean
        Return objectValue >= StartDate AndAlso objectValue <= EndDate
    End Function

    Overrides ReadOnly Property Description As String
        Get
            Return $"{DisplayPropertyName} entre {StartDate.ToString("dd/MM/YY")} y {EndDate.ToString("dd/MM/YY")}"
        End Get
    End Property

End Class
