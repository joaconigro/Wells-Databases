Public Class PremadeSeriesInfo
    Property ParameterGroup As String
    Property PropertyDisplayName As String
    Property IsFromWell As String

    Public Overrides Function ToString() As String
        If IsFromWell Then
            Return PropertyDisplayName
        End If
        Return "Precipitaciones"
    End Function
End Class
