Public Class PremadeSeriesInfoCollection
    Property Title As String
    Property Values As List(Of PremadeSeriesInfo)

    Sub New()
        Values = New List(Of PremadeSeriesInfo)
    End Sub

    Sub Add(item As PremadeSeriesInfo)
        Values.Add(item)
    End Sub

    Sub Remove(item As PremadeSeriesInfo)
        Values.Remove(item)
    End Sub

    Public Overrides Function ToString() As String
        Return Title
    End Function
End Class
