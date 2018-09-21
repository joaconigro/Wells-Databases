Imports System.ComponentModel

Public Class ExternalLink
    Inherits BusinessObject

    <DisplayName("Ruta"), Browsable(True)>
    Property Link As String

    <Browsable(False)>
    Property Well As Well

    <Browsable(False)>
    ReadOnly Property WellId As String
        Get
            Return Well?.Id
        End Get
    End Property

    Private _WellName As String
    <DisplayName("Pozo"), Browsable(True)>
    Property WellName As String
        Get
            If Well IsNot Nothing Then
                Return Well.Name
            Else
                Return _WellName
            End If
        End Get
        Set
            _WellName = Value
        End Set
    End Property

    Sub Open()
        Try
            Process.Start(Link)
        Catch ex As Exception
            Throw New Exception("No se puede abrir el enlace externo. Más información: " & ex.Message)
        End Try
    End Sub
End Class
