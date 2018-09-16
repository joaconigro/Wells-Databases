Public Interface IMainWindowView
    Function OpenFileDialog(filter As String, title As String) As String
    Function SaveFileDialog(filter As String, title As String, Optional filename As String = "") As String
End Interface
