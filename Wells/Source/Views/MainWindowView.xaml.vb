﻿Imports Wells.ViewBase

Class MainWindowView
    Implements IMainWindowView

    Private viewModel As MainWindowViewModel

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()


        ' Add any initialization after the InitializeComponent() call.
        viewModel = New MainWindowViewModel(Me)
        ViewWellMenuItem.IsChecked = If(My.Settings.ShowedDatasource = 0, True, False)
        ViewMeasurementsMenuItem.IsChecked = If(My.Settings.ShowedDatasource = 1, True, False)
        ViewFLNAAnalysisMenuItem.IsChecked = If(My.Settings.ShowedDatasource = 2, True, False)
        ViewWaterAnalysisMenuItem.IsChecked = If(My.Settings.ShowedDatasource = 3, True, False)
        ViewSoilAnalysisMenuItem.IsChecked = If(My.Settings.ShowedDatasource = 4, True, False)
        ViewPrecipitationsMenuItem.IsChecked = If(My.Settings.ShowedDatasource = 5, True, False)
        StartDatePicker.SelectedDate = New Date(2000, 1, 1)
        EndDatePicker.SelectedDate = Today

        DataContext = viewModel

    End Sub

    Public Function ShowMessageBox(message As String, title As String) As Boolean Implements IView.ShowMessageBox
        Return SharedBaseView.ShowMessageBox(Me, message, title)
    End Function

    Public Sub ShowErrorMessageBox(message As String) Implements IView.ShowErrorMessageBox
        SharedBaseView.ShowErrorMessageBox(Me, message)
    End Sub

    'Public Function OpenFileDialog(filter As String, title As String) As String Implements IView.OpenFileDialog
    '    Return SharedBaseView.OpenFileDialog(filter, title)
    'End Function

    'Public Function SaveFileDialog(filter As String, title As String, Optional filename As String = "") As String Implements IView.SaveFileDialog
    '    Return SharedBaseView.SaveFileDialog(filter, title, filename)
    'End Function

    Public Function CreateDatabaseDialog(ByRef databaseName As String) As Boolean Implements IMainWindowView.CreateDatabaseDialog
        Dim diag As New CreateDatabaseDialog() With {.Owner = Me}
        If diag.ShowDialog Then
            databaseName = diag.DatabaseName
            'Path = diag.DatabasePath
            Return True
        End If
        Return False
    End Function

    Public Function SelectSheetDialog(sheets As List(Of String)) As Integer Implements IMainWindowView.SelectSheetDialog
        Dim diag As New ExcelSheetsDialog(sheets) With {.Owner = Me}
        If diag.ShowDialog Then
            Return diag.SelectedSheet
        End If
        Return -1
    End Function

    Public Function ShowEditWellDialog(vm As EditWellViewModel) As Boolean Implements IMainWindowView.ShowEditWellDialog
        Dim diag As New WellEditingDialog(vm)
        Return diag.ShowDialog()
    End Function

    Public Function ShowEditMeasurementDialog(vm As EditMeasurementViewModel) As Boolean Implements IMainWindowView.ShowEditMeasurementDialog
        Dim diag As New EditMeasurementDialog(vm)
        Return diag.ShowDialog()
    End Function

    Public Sub OpenGraphicsView(vm As GraphicsViewModel) Implements IMainWindowView.OpenGraphicsView
        Dim diag As New GraphicsView(vm)
        diag.Show()
    End Sub

    Public Function OpenFileDialog(filter As String, title As String, Optional initialDirectory As String = "") As String Implements IView.OpenFileDialog
        Return SharedBaseView.OpenFileDialog(filter, title, initialDirectory)
    End Function

    Public Sub ShowOkOnkyMessageBox(message As String, title As String) Implements IView.ShowOkOnkyMessageBox
        Throw New NotImplementedException()
    End Sub

    Public Function SaveFileDialog(filter As String, title As String, Optional filename As String = "", Optional initialDirectory As String = "") As String Implements IView.SaveFileDialog
        Return SharedBaseView.SaveFileDialog(filter, title, filename, initialDirectory)
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
