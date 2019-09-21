Public Interface IBaseFilter
    Property DisplayPropertyName As String
    ReadOnly Property DisplayValue As String
    ReadOnly Property Description As String
    Property IsEnabled As Boolean
    Property PropertyName As String
    ReadOnly Property IsDateRangeFilter As Boolean
End Interface
