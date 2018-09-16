Public Class CreateDatabaseDialog

    Private _DatabaseName As String
    Private _DatabasePath As String

    Property DatabaseName As String
        Get
            Return _DatabaseName
        End Get
        Set
            _DatabaseName = Value
            Validate()
        End Set
    End Property

    Property DatabasePath As String
        Get
            Return _DatabasePath
        End Get
        Set
            _DatabasePath = Value
            Validate()
        End Set
    End Property

    Private Sub Validate()
        If Not String.IsNullOrEmpty(_DatabaseName) AndAlso Not String.IsNullOrEmpty(_DatabasePath) Then
            OkButton.IsEnabled = True
        Else
            OkButton.IsEnabled = False
        End If
    End Sub

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Validate()
        DataContext = Me
    End Sub

    Private Sub ChooseDatabasePathButtonClicked(sender As Object, e As RoutedEventArgs) Handles ChooseDatabasePathButton.Click
        Dim diag As New Forms.FolderBrowserDialog()
        If diag.ShowDialog() = Forms.DialogResult.OK Then
            DatabasePathTextBox.Text = diag.SelectedPath
            _DatabasePath = diag.SelectedPath
        End If
        Validate()
    End Sub

    Private Sub OkButtonClicked(sender As Object, e As RoutedEventArgs) Handles OkButton.Click
        DialogResult = True
    End Sub

    Private Sub CancelButtonClicked(sender As Object, e As RoutedEventArgs) Handles CancelButton.Click
        DialogResult = False
    End Sub
End Class
