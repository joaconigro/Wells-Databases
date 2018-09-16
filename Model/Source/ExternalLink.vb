Public Class ExternalLink
    Inherits BusinessObject

    Property Link As String
    Property Well As Well
    Property WellId As String

    Sub Open()
        Try
            Process.Start(Link)
        Catch ex As Exception
            Throw New Exception("No se puede abrir el enlace externo. Más información: " & ex.Message)
        End Try
    End Sub
End Class
