Imports Wells.Model
Imports Wells.Persistence

Class MainWindow

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Dim db As New Repository

        Dim w As New Well("Pozo prueba")
        db.Wells.add(w)
        db.SaveChanges()

    End Sub
End Class
