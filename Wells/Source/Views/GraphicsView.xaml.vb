Imports Wells

Public Class GraphicsView
    Implements IView

    Private _viewModel As GraphicsViewModel

    Sub New(viewmodel As GraphicsViewModel)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _viewModel = viewmodel
        'MainChart.Series.Clear()
        DataContext = _viewModel

    End Sub

    Public Sub ShowErrorMessageBox(message As String) Implements IView.ShowErrorMessageBox
        MessageBox.Show(Me, message, "Error", MessageBoxButton.OK, MessageBoxImage.Error)
    End Sub

    Public Function OpenFileDialog(filter As String, title As String) As String Implements IView.OpenFileDialog
        Return String.Empty
    End Function

    Public Function ShowMessageBox(message As String, title As String) As Boolean Implements IView.ShowMessageBox
        Return MessageBox.Show(Me, message, title, MessageBoxButton.YesNo, MessageBoxImage.Information) = MessageBoxResult.Yes
    End Function
End Class
