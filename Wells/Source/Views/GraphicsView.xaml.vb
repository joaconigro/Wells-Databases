Imports Wells

Public Class GraphicsView
    Implements IView

    Private _viewModel As GraphicsViewModel

    Sub New(viewmodel As GraphicsViewModel)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _viewModel = viewmodel
        DataContext = _viewModel
    End Sub

    Public Function ShowMessageBox(message As String, title As String) As Boolean Implements IView.ShowMessageBox
        Return SharedBaseView.ShowMessageBox(Me, message, title)
    End Function

    Public Sub ShowErrorMessageBox(message As String) Implements IView.ShowErrorMessageBox
        SharedBaseView.ShowErrorMessageBox(Me, message)
    End Sub

    Public Function OpenFileDialog(filter As String, title As String) As String Implements IView.OpenFileDialog
        Return String.Empty
    End Function

    Public Function SaveFileDialog(filter As String, title As String, Optional filename As String = "") As String Implements IView.SaveFileDialog
        Return String.Empty
    End Function
End Class
