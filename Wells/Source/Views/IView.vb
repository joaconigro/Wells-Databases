Public Interface IView
    Function OpenFileDialog(filter As String, title As String) As String
    Function ShowMessageBox(message As String, title As String) As Boolean
    Sub ShowErrorMessageBox(message As String)
    Function SaveFileDialog(filter As String, title As String, Optional filename As String = "") As String
End Interface
