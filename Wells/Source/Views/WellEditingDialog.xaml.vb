Imports Wells

Public Class WellEditingDialog
    Implements IView

    Private _viewModel As EditWellViewModel

    Sub New(vm As EditWellViewModel)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        DataContext = vm
        _viewModel = vm
        _viewModel.View = Me

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
            Case NameOf(_viewModel.Analysis)
                AnalysisEntityControl.EntitiesListview.Items.Refresh()
            Case NameOf(_viewModel.Links)
                ExternalLinksEntityControl.EntitiesListview.Items.Refresh()
        End Select
    End Sub

    Public Function ShowMessageBox(message As String, title As String) As Boolean Implements IView.ShowMessageBox
        Return MessageBox.Show(Me, message, title, MessageBoxButton.YesNo, MessageBoxImage.Information) = MessageBoxResult.Yes
    End Function

    Public Sub ShowErrorMessageBox(message As String) Implements IView.ShowErrorMessageBox
        MessageBox.Show(Me, message, "Error", MessageBoxButton.OK, MessageBoxImage.Error)
    End Sub

    Public Function ShowEditMeasurementDialog(vm As EditMeasurementViewModel) As Boolean
        Dim diag As New EditMeasurementDialog(vm)
        Return diag.ShowDialog()
    End Function

    Public Function OpenFileDialog(filter As String, title As String) As String Implements IView.OpenFileDialog
        Dim ofd As New Microsoft.Win32.OpenFileDialog With {.Filter = filter, .Title = title}
        If ofd.ShowDialog = True Then
            Return ofd.FileName
        End If
        Return Nothing
    End Function
End Class
