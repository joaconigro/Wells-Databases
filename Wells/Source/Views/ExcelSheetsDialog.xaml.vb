Public Class ExcelSheetsDialog

    Private _sheets As List(Of String)

    Property SelectedSheet As Integer = 0

    Sub New(sheets As List(Of String))

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        DataContext = Me
        _sheets = sheets

        For Each s In _sheets
            SheetsComoboBox.Items.Add(s)
        Next
    End Sub

    Private Sub OkButtonClicked(sender As Object, e As RoutedEventArgs) Handles OkButtom.Click
        DialogResult = True
    End Sub

    Private Sub CancelButtonClicked(sender As Object, e As RoutedEventArgs) Handles CanelButtom.Click
        DialogResult = False
    End Sub
End Class
