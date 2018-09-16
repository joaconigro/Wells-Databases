Imports Wells

Class MainWindowView
    Implements IMainWindowView

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()


        ' Add any initialization after the InitializeComponent() call.
        DataContext = New MainWindowViewModel(Me)

    End Sub

    Public Function OpenFileDialog(filter As String, title As String) As String Implements IMainWindowView.OpenFileDialog
        Dim ofd As New Microsoft.Win32.OpenFileDialog With {.Filter = filter, .Title = title}
        If ofd.ShowDialog = True Then
            Return ofd.FileName
        End If
        Return Nothing
    End Function

    Public Function SaveFileDialog(filter As String, title As String, Optional filename As String = "") As String Implements IMainWindowView.SaveFileDialog
        Dim sfd As New Microsoft.Win32.SaveFileDialog With {.Filter = filter, .Title = title, .FileName = filename}
        If sfd.ShowDialog = True Then
            Return sfd.FileName
        End If
        Return Nothing
    End Function

    Public Function CreateDatabaseDialog(ByRef databaseName As String, ByRef path As String) As Boolean Implements IMainWindowView.CreateDatabaseDialog
        Dim diag As New CreateDatabaseDialog()
        If diag.ShowDialog Then
            databaseName = diag.DatabaseName
            path = diag.DatabasePath
            Return True
        End If
        Return False
    End Function
End Class
