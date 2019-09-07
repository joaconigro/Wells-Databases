Imports Wells.ViewBase

Public Class EditMeasurementDialog
    Implements IView

    Private _viewModel As EditMeasurementViewModel

    Sub New(vm As EditMeasurementViewModel)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        DataContext = vm
        _viewModel = vm
        _viewModel.SetView(Me)

        AddHandler _viewModel.CloseDialog, AddressOf CloseDialog
    End Sub

    Sub CloseDialog(result As Boolean)
        RemoveHandler _viewModel.CloseDialog, AddressOf CloseDialog
        DialogResult = result
        Close()
    End Sub

    Public Function ShowMessageBox(message As String, title As String) As Boolean Implements IView.ShowMessageBox
        Return SharedBaseView.ShowMessageBox(Me, message, title)
    End Function

    Public Sub ShowErrorMessageBox(message As String) Implements IView.ShowErrorMessageBox
        SharedBaseView.ShowErrorMessageBox(Me, message)
    End Sub

    'Public Function OpenFileDialog(filter As String, title As String) As String Implements IView.OpenFileDialog
    '    Return String.Empty
    'End Function

    'Public Function SaveFileDialog(filter As String, title As String, Optional filename As String = "") As String Implements IView.SaveFileDialog
    '    Return String.Empty
    'End Function

    Public Function OpenFileDialog(filter As String, title As String, Optional initialDirectory As String = "") As String Implements IView.OpenFileDialog
        Throw New NotImplementedException()
    End Function

    Public Sub ShowOkOnkyMessageBox(message As String, title As String) Implements IView.ShowOkOnkyMessageBox
        Throw New NotImplementedException()
    End Sub

    Public Function SaveFileDialog(filter As String, title As String, Optional filename As String = "", Optional initialDirectory As String = "") As String Implements IView.SaveFileDialog
        Throw New NotImplementedException()
    End Function

    Public Function ShowInputBox(prompt As String, Optional title As String = "", Optional defaultResponse As String = "") As String Implements IView.ShowInputBox
        Throw New NotImplementedException()
    End Function

    Public Sub CloseView(dialogResult As Boolean?) Implements IView.CloseView
        Throw New NotImplementedException()
    End Sub

    Public Sub CloseView() Implements IView.CloseView
        Throw New NotImplementedException()
    End Sub

    Public Function ShowFolderSelectionDialog() As String Implements IView.ShowFolderSelectionDialog
        Throw New NotImplementedException()
    End Function
End Class
