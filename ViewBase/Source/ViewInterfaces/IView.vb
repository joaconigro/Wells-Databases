Public Interface IView
    Function OpenFileDialog(filter As String, title As String, Optional initialDirectory As String = "") As String
    Function ShowMessageBox(message As String, title As String) As Boolean
    Sub ShowErrorMessageBox(message As String)
    Sub ShowOkOnkyMessageBox(message As String, title As String)
    Function SaveFileDialog(filter As String, title As String, Optional filename As String = "", Optional initialDirectory As String = "") As String
    Function ShowInputBox(prompt As String, Optional title As String = "", Optional defaultResponse As String = "") As String
    Sub CloseView(dialogResult As Boolean?)
    Sub CloseView()
    Function ShowFolderSelectionDialog() As String
End Interface
