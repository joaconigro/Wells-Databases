Imports Wells.ViewBase
Public Interface IMainWindowView
    Inherits IView
    Function CreateDatabaseDialog(ByRef databaseName As String) As Boolean
    Function SelectSheetDialog(sheets As List(Of String)) As Integer
    Sub ShowWaitingMessage(message As String)
    Sub CloseWaitingMessage()
    Sub OpenGraphicsView()
    Function OpenEditEntityDialog(vm As EditWellViewModel) As Boolean
    Function OpenEditEntityDialog(vm As EditMeasurementViewModel) As Boolean
End Interface
