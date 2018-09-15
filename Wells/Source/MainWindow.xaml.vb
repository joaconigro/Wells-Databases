Imports Wells.Model
Imports Wells.Persistence

Class MainWindow

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Dim db As New Repository()


        MainDataGrid.ItemsSource = db.Wells
    End Sub
End Class
