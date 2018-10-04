Public Interface IMainWindowView
    Inherits IView
    Function CreateDatabaseDialog(ByRef databaseName As String, ByRef path As String) As Boolean
    Function SaveFileDialog(filter As String, title As String, Optional filename As String = "") As String
    Function SelectSheetDialog(sheets As List(Of String)) As Integer
    Function ShowEditWellDialog(vm As EditWellViewModel) As Boolean
    Function ShowEditMeasurementDialog(vm As EditMeasurementViewModel) As Boolean
    Sub OpenGraphicsView(vm As GraphicsViewModel)
End Interface
