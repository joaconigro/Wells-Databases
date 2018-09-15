Imports Wells
Imports Wells.Model
Imports Wells.Persistence

Class MainWindow
    Implements IMainWindow

    Private repo As Repository

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        If Not String.IsNullOrEmpty(My.Settings.DatabaseFilename) AndAlso IO.File.Exists(My.Settings.DatabaseFilename) Then
            repo = New Repository(My.Settings.DatabaseFilename, False)
            MainDataGrid.ItemsSource = repo.Wells.Values.ToList
        End If

    End Sub

    Public Function OpenFileDialog(filter As String, title As String) As String Implements IMainWindow.OpenFileDialog
        Dim ofd As New Microsoft.Win32.OpenFileDialog With {.Filter = filter, .Title = title}
        If ofd.ShowDialog = True Then
            Return ofd.FileName
        End If
        Return Nothing
    End Function

    Public Function SaveFileDialog(filter As String, title As String, Optional filename As String = "") As String Implements IMainWindow.SaveFileDialog
        Dim sfd As New Microsoft.Win32.SaveFileDialog With {.Filter = filter, .Title = title, .FileName = filename}
        If sfd.ShowDialog = True Then
            Return sfd.FileName
        End If
        Return Nothing
    End Function

    Private Sub NewDatabaseMenuItemClicked(sender As Object, e As RoutedEventArgs) Handles NewDatabaseMenuItem.Click
        Dim filename = SaveFileDialog("Well Databases|*.mdf", "Nueva base de datos")
        If Not String.IsNullOrEmpty(filename) Then
            My.Settings.DatabaseFilename = filename
            My.Settings.Save()
            repo = New Repository(filename, True)
            MainDataGrid.ItemsSource = repo.Wells.Values.ToList
        End If
    End Sub

    Private Sub NewWellMenuItemClicked(sender As Object, e As RoutedEventArgs) Handles NewWellMenuItem.Click
        Dim w As New Well With {.DateMade = Today, .Name = "P1"}
        repo.Add(w)
    End Sub
End Class
