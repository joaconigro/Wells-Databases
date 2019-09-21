Imports Wells.ViewBase

Public Class WellEditingDialog
    Implements IView

    Private _viewModel As EditWellViewModel

    Sub New(vm As EditWellViewModel)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        DataContext = vm
        _viewModel = vm
        _viewModel.SetView(Me)

        ExternalLinksEntityControl.EditEntityButton.Content = "Abrir"
        ExternalLinksEntityControl.NewEntityButton.Content = "Nuevo"

        AddHandler _viewModel.CloseDialog, AddressOf CloseDialog
        AddHandler _viewModel.MustRebind, AddressOf OnRebind
    End Sub

    Sub CloseDialog(result As Boolean)
        RemoveHandler _viewModel.CloseDialog, AddressOf CloseDialog
        RemoveHandler _viewModel.MustRebind, AddressOf OnRebind
        DialogResult = result
        Close()
    End Sub

    Private Sub OnRebind(target As String)
        Select Case target
            Case NameOf(_viewModel.Measurements)
                MeasurementsEntityControl.EntitiesListview.Items.Refresh()
            Case NameOf(_viewModel.SoilAnalyses)
                SoilAnalysisEntityControl.EntitiesListview.Items.Refresh()
            Case NameOf(_viewModel.WaterAnalyses)
                WaterAnalysisEntityControl.EntitiesListview.Items.Refresh()
            Case NameOf(_viewModel.FLNAAnalyses)
                FLNAAnalysisEntityControl.EntitiesListview.Items.Refresh()
            Case NameOf(_viewModel.Files)
                ExternalLinksEntityControl.EntitiesListview.Items.Refresh()
        End Select
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

    Public Function ShowEditMeasurementDialog(vm As EditMeasurementViewModel) As Boolean
        Dim diag As New EditMeasurementDialog(vm)
        Return diag.ShowDialog()
    End Function

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
