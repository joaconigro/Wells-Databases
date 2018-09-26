Public Interface IView
    Function ShowMessageBox(message As String, title As String) As Boolean
    Sub ShowErrorMessageBox(message As String)
End Interface
