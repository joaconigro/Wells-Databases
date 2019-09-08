Imports Wells.ViewBase
Public Interface IMainWindowView
    Inherits IView
    Function CreateDatabaseDialog(ByRef databaseName As String) As Boolean
    Function SelectSheetDialog(sheets As List(Of String)) As Integer
    Function ShowEditWellDialog(vm As EditWellViewModel) As Boolean
    Function ShowEditMeasurementDialog(vm As EditMeasurementViewModel) As Boolean
    Sub OpenGraphicsView(vm As GraphicsViewModel)
End Interface
