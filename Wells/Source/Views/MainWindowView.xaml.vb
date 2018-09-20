Imports Wells

Class MainWindowView
    Implements IMainWindowView

    Private viewModel As MainWindowViewModel

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()


        ' Add any initialization after the InitializeComponent() call.
        StartDatePicker.SelectedDate = New Date(2000, 1, 1)
        EndDatePicker.SelectedDate = Today
        viewModel = New MainWindowViewModel(Me)
        ViewWellMenuItem.IsChecked = If(My.Settings.ShowedDatasource = 0, True, False)
        ViewMeasurementsMenuItem.IsChecked = If(My.Settings.ShowedDatasource = 1, True, False)
        ViewAnalysisMenuItem.IsChecked = If(My.Settings.ShowedDatasource = 2, True, False)
        ViewPrecipitationsMenuItem.IsChecked = If(My.Settings.ShowedDatasource = 3, True, False)

        DataContext = viewModel

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

    Private Sub ViewWellMenuItemClicked(sender As Object, e As RoutedEventArgs) Handles ViewWellMenuItem.Click
        ViewWellMenuItem.IsChecked = True
        ViewMeasurementsMenuItem.IsChecked = False
        ViewAnalysisMenuItem.IsChecked = False
        ViewPrecipitationsMenuItem.IsChecked = False
        viewModel.Filter.ShowedDatasource = DatasourceType.Wells
    End Sub

    Private Sub ViewMeasurementsMenuItemClicked(sender As Object, e As RoutedEventArgs) Handles ViewMeasurementsMenuItem.Click
        ViewWellMenuItem.IsChecked = False
        ViewMeasurementsMenuItem.IsChecked = True
        ViewAnalysisMenuItem.IsChecked = False
        ViewPrecipitationsMenuItem.IsChecked = False
        viewModel.Filter.ShowedDatasource = DatasourceType.Measurements
    End Sub

    Private Sub ViewAnalysisMenuItemClicked(sender As Object, e As RoutedEventArgs) Handles ViewAnalysisMenuItem.Click
        ViewWellMenuItem.IsChecked = False
        ViewMeasurementsMenuItem.IsChecked = False
        ViewAnalysisMenuItem.IsChecked = True
        ViewPrecipitationsMenuItem.IsChecked = False
        viewModel.Filter.ShowedDatasource = DatasourceType.ChemicalAnalysis
    End Sub

    Private Sub ViewPrecipitationsMenuItemClicked(sender As Object, e As RoutedEventArgs) Handles ViewPrecipitationsMenuItem.Click
        ViewWellMenuItem.IsChecked = False
        ViewMeasurementsMenuItem.IsChecked = False
        ViewAnalysisMenuItem.IsChecked = False
        ViewPrecipitationsMenuItem.IsChecked = True
        viewModel.Filter.ShowedDatasource = DatasourceType.Precipitations
    End Sub
End Class
