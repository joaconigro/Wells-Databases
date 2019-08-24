Imports System.Windows

Public Class ResourceFinder

    Private Shared ReadOnly _Languages As New Dictionary(Of String, ResourceDictionary) From {
        {"en", New EnglishLanguage},
        {"es", New SpanishLanguage}}

    Private Shared _LanguageCode As String

    Shared Sub SetLanguage(code As String)
        Dim lowerCode = code.ToLower
        If Not _Languages.ContainsKey(lowerCode) Then
            lowerCode = "en"
        End If
        _LanguageCode = lowerCode
    End Sub

    Shared Function FindResource(key As String) As String
        If _Languages(_LanguageCode).Contains(key) Then
            Return _Languages(_LanguageCode)(key).ToString
        End If
        Return Nothing
    End Function

End Class
