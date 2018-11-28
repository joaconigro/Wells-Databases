Imports Wells

Class MainWindowView
    Implements IMainWindowView

    Private viewModel As MainWindowViewModel

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()


        ' Add any initialization after the InitializeComponent() call.
        viewModel = New MainWindowViewModel(Me)
        ViewWellMenuItem.IsChecked = If(My.Settings.ShowedDatasource = 0, True, False)
        ViewMeasurementsMenuItem.IsChecked = If(My.Settings.ShowedDatasource = 1, True, False)
        ViewFLNAAnalysisMenuItem.IsChecked = If(My.Settings.ShowedDatasource = 2, True, False)
        ViewWaterAnalysisMenuItem.IsChecked = If(My.Settings.ShowedDatasource = 3, True, False)
        ViewSoilAnalysisMenuItem.IsChecked = If(My.Settings.ShowedDatasource = 4, True, False)
        ViewPrecipitationsMenuItem.IsChecked = If(My.Settings.ShowedDatasource = 5, True, False)
        StartDatePicker.SelectedDate = New Date(2000, 1, 1)
        EndDatePicker.SelectedDate = Today

        DataContext = viewModel

    End Sub

    Public Function ShowMessageBox(message As String, title As String) As Boolean Implements IView.ShowMessageBox
        Return SharedBaseView.ShowMessageBox(Me, message, title)
    End Function

    Public Sub ShowErrorMessageBox(message As String) Implements IView.ShowErrorMessageBox
        SharedBaseView.ShowErrorMessageBox(Me, message)
    End Sub

    Public Function OpenFileDialog(filter As String, title As String) As String Implements IView.OpenFileDialog
        Return SharedBaseView.OpenFileDialog(filter, title)
    End Function

    Public Function SaveFileDialog(filter As String, title As String, Optional filename As String = "") As String Implements IView.SaveFileDialog
        Return SharedBaseView.SaveFileDialog(filter, title, filename)
    End Function

    Public Function CreateDatabaseDialog(ByRef databaseName As String, ByRef path As String) As Boolean Implements IMainWindowView.CreateDatabaseDialog
        Dim diag As New CreateDatabaseDialog() With {.Owner = Me}
        If diag.ShowDialog Then
            databaseName = diag.DatabaseName
            path = diag.DatabasePath
            Return True
        End If
        Return False
    End Function

    Public Function SelectSheetDialog(sheets As List(Of String)) As Integer Implements IMainWindowView.SelectSheetDialog
        Dim diag As New ExcelSheetsDialog(sheets) With {.Owner = Me}
        If diag.ShowDialog Then
            Return diag.SelectedSheet
        End If
        Return -1
    End Function

    Public Function ShowEditWellDialog(vm As EditWellViewModel) As Boolean Implements IMainWindowView.ShowEditWellDialog
        Dim diag As New WellEditingDialog(vm)
        Return diag.ShowDialog()
    End Function

    Public Function ShowEditMeasurementDialog(vm As EditMeasurementViewModel) As Boolean Implements IMainWindowView.ShowEditMeasurementDialog
        Dim diag As New EditMeasurementDialog(vm)
        Return diag.ShowDialog()
    End Function

    Public Sub OpenGraphicsView(vm As GraphicsViewModel) Implements IMainWindowView.OpenGraphicsView
        Dim diag As New GraphicsView(vm)
        diag.Show()
    End Sub
End Class
