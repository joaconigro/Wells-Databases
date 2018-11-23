
Public Class SharedBaseView
    Inherits Window

    Public Shared Sub ShowErrorMessageBox(owner As Window, message As String)
        MessageBox.Show(owner, message, "Error", MessageBoxButton.OK, MessageBoxImage.Error)
    End Sub

    Public Shared Function OpenFileDialog(filter As String, title As String) As String
        Dim ofd As New Microsoft.Win32.OpenFileDialog With {.Filter = filter, .Title = title}
        If ofd.ShowDialog = True Then
            Return ofd.FileName
        End If
        Return Nothing
    End Function

    Public Shared Function SaveFileDialog(filter As String, title As String, Optional filename As String = "") As String
        Dim sfd As New Microsoft.Win32.SaveFileDialog With {.Filter = filter, .Title = title, .FileName = filename}
        If sfd.ShowDialog = True Then
            Return sfd.FileName
        End If
        Return Nothing
    End Function

    Public Shared Function ShowMessageBox(owner As Window, message As String, title As String) As Boolean
        Return MessageBox.Show(owner, message, title, MessageBoxButton.YesNo, MessageBoxImage.Information) = MessageBoxResult.Yes
    End Function
End Class
