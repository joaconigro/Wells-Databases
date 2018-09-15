Imports Wells
Imports Wells.Model
Imports Wells.Persistence

Class MainWindow
    Implements IMainWindow

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        Dim db As Repository

        'Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\WellDB.mdf;Integrated Security=True
        Dim databasefile As String
        If Not IO.File.Exists("D:\WellDB.mdf") Then
            Dim ofd As New Microsoft.Win32.OpenFileDialog With {.Filter = "Databases|*.mdf"}
            If ofd.ShowDialog = True Then
                databasefile = ofd.FileName
                Dim conString = $"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={databasefile};Integrated Security=True"
                db = New Repository(conString)
            End If
        Else
            db = New Repository("name=WellDB")
        End If



        MainDataGrid.ItemsSource = db.Wells
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
End Class
