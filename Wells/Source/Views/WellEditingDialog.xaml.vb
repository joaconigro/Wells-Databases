Public Class WellEditingDialog
    Implements IView

    Private _viewModel As EditWellViewModel

    Sub New(vm As EditWellViewModel)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        DataContext = vm
        _viewModel = vm
        _viewModel.View = Me
        AddHandler _viewModel.CloseDialog, AddressOf CloseDialog
    End Sub

    Sub CloseDialog(result As Boolean)
        RemoveHandler _viewModel.CloseDialog, AddressOf CloseDialog
        DialogResult = result
        Close()
    End Sub

    Public Function ShowMessageBox(message As String, title As String) As Boolean Implements IView.ShowMessageBox
        Return MessageBox.Show(Me, message, title, MessageBoxButton.YesNo, MessageBoxImage.Information) = MessageBoxResult.Yes
    End Function

    Public Sub ShowErrorMessageBox(message As String) Implements IView.ShowErrorMessageBox
        MessageBox.Show(Me, message, "Error", MessageBoxButton.OK, MessageBoxImage.Error)
    End Sub
End Class
