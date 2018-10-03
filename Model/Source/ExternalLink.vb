Imports System.ComponentModel
Imports System.IO

Public Class ExternalLink
    Inherits BusinessObject
    Implements IComparable(Of ExternalLink)

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

    Public Overrides Function ToString() As String
        If File.Exists(Link) Then
            Return Path.GetFileName(Link)
        End If
        Return Link
    End Function

    Sub Open()
        Try
            Process.Start(Link)
        Catch ex As Exception
            Throw New Exception("No se puede abrir el enlace externo. Más información: " & ex.Message)
        End Try
    End Sub

    Sub DeleteFile()
        Try
            If File.Exists(Link) Then
                File.Delete(Link)
            End If
        Catch ex As Exception
            'Throw New Exception("No se puede borrar el enlace externo. Más información: " & ex.Message)
        End Try
    End Sub

    Public Function CompareTo(other As ExternalLink) As Integer Implements IComparable(Of ExternalLink).CompareTo
        If Me.ToString > other.Link.ToString Then Return -1
        If Me.ToString = other.Link.ToString Then Return 0
        Return 1
    End Function
End Class
