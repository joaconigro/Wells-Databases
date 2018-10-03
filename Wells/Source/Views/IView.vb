Public Interface IView
    Function OpenFileDialog(filter As String, title As String) As String
    Function ShowMessageBox(message As String, title As String) As Boolean
    Sub ShowErrorMessageBox(message As String)
End Interface
