Imports System.ComponentModel

Public Class ReflectionExtension

    Shared Function GetDisplayName(Of T As Class)(propertyName As Object) As String
        Dim member = GetType(T).GetProperty(propertyName)
        Dim dn = member.GetCustomAttributes(GetType(DisplayNameAttribute), False)
        If dn IsNot Nothing Then
            Return CType(dn.First, DisplayNameAttribute).DisplayName
        End If
        Return String.Empty
    End Function

End Class
