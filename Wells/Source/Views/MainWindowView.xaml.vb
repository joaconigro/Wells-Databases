Imports Wells
Imports Wells.ViewBase

Class MainWindowView
    Implements IMainWindowView

    Private _ViewModel As MainWindowViewModel
    Private _WaitingDialog As WaitingView

    Event PremadeGraphicsChanged() Implements IMainWindowView.PremadeGraphicsChanged

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _ViewModel = New MainWindowViewModel(Me)
        DataContext = _ViewModel

    End Sub

#Region "IView implementation"
    Public Sub ShowErrorMessageBox(message As String) Implements IView.ShowErrorMessageBox
        SharedBaseView.ShowErrorMessageBox(Me, message)
    End Sub

    Public Sub ShowOkOnkyMessageBox(message As String, title As String) Implements IView.ShowOkOnkyMessageBox
        SharedBaseView.ShowOkOnkyMessageBox(Me, message, title)
    End Sub

    Public Sub CloseView(dialogResult As Boolean?) Implements IView.CloseView
        Me.DialogResult = dialogResult
        Close()
    End Sub

    Public Sub CloseView() Implements IView.CloseView
        Close()
    End Sub

    Public Function OpenFileDialog(filter As String, title As String, Optional initialDirectory As String = "") As String Implements IView.OpenFileDialog
        Return SharedBaseView.OpenFileDialog(filter, title, initialDirectory)
    End Function

    Public Function ShowMessageBox(message As String, title As String) As Boolean Implements IView.ShowMessageBox
        Return SharedBaseView.ShowMessageBox(Me, message, title)
    End Function

    Public Function SaveFileDialog(filter As String, title As String, Optional filename As String = "", Optional initialDirectory As String = "") As String Implements IView.SaveFileDialog
        Return SharedBaseView.SaveFileDialog(filter, title, filename, initialDirectory)
    End Function

    Public Function ShowInputBox(prompt As String, Optional title As String = "", Optional defaultResponse As String = "") As String Implements IView.ShowInputBox
        Return SharedBaseView.ShowInputBox(prompt, title, defaultResponse)
    End Function

    Public Function ShowFolderSelectionDialog() As String Implements IView.ShowFolderSelectionDialog
        Return SharedBaseView.ShowFolderSelectionDialog()
    End Function


#End Region

    Private Sub CreateViewModelsForPanels()
        Application.Current.Dispatcher.Invoke(Sub()
                                                  WellsControl.SetViewModel(New WellsViewModel)
                                                  MeasurementsControl.SetViewModel(New MeasurementsViewModel)
                                                  WaterAnalysesControl.SetViewModel(New WaterAnalysesViewModel)
                                                  SoilAnalysesControl.SetViewModel(New SoilAnalysesViewModel)
                                                  FLNAAnalysesControl.SetViewModel(New FLNAAnalysesViewModel)
                                                  PrecipitationsControl.SetViewModel(New PrecipitationsViewModel)
                                              End Sub)
    End Sub

    Private Sub AfterContentRendered(sender As Object, e As EventArgs)
        If Not String.IsNullOrEmpty(My.Settings.DatabaseFilename) Then
            Task.Run(Sub()
                         _ViewModel.InitializeContext(My.Settings.DatabaseFilename, False)
                         CreateViewModelsForPanels()
                         CloseWaitingMessage()
                     End Sub)
        End If
    End Sub

#Region "IMainWindow implementation"
    Public Function CreateDatabaseDialog(ByRef databaseName As String) As Boolean Implements IMainWindowView.CreateDatabaseDialog
        Dim diag As New CreateDatabaseDialog() With {.Owner = Me}
        If diag.ShowDialog Then
            databaseName = diag.DatabaseName
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

    Public Sub ShowWaitingMessage(message As String) Implements IMainWindowView.ShowWaitingMessage
        Application.Current.Dispatcher.Invoke(Sub()
                                                  IsEnabled = False
                                                  _WaitingDialog = New WaitingView(message) With {.Owner = Me}
                                                  _WaitingDialog.Show()
                                              End Sub)
    End Sub

    Public Sub CloseWaitingMessage() Implements IMainWindowView.CloseWaitingMessage
        Application.Current.Dispatcher.Invoke(Sub()
                                                  _WaitingDialog?.Close()
                                                  _WaitingDialog = Nothing
                                                  IsEnabled = True
                                              End Sub)
    End Sub

    Public Sub OpenGraphicsView() Implements IMainWindowView.OpenGraphicsView
        Dim diag As New CustomGraphicsView()
        diag.Show()
    End Sub

    Function OpenEditEntityDialog(vm As EditWellViewModel) As Boolean Implements IMainWindowView.OpenEditEntityDialog
        Dim diag As New WellEditingDialog(vm) With {.Owner = Me}
        Return diag.ShowDialog()
    End Function

    Function OpenEditEntityDialog(vm As EditMeasurementViewModel) As Boolean Implements IMainWindowView.OpenEditEntityDialog
        Dim diag As New EditMeasurementDialog(vm) With {.Owner = Me}
        Return diag.ShowDialog()
    End Function

    Public Sub OpenCreatePremadeGraphicView() Implements IMainWindowView.OpenCreatePremadeGraphicView
        Dim diag = New CreatePremadeGraphicView() With {.Owner = Me}
        diag.ShowDialog()
        RaiseEvent PremadeGraphicsChanged()
    End Sub

    Public Sub OpenGraphicsView(well As YPFModel.Well, series As PremadeSeriesInfoCollection) Implements IMainWindowView.OpenGraphicsView
        Dim diag = New PremadeGraphicsView(well, series) With {.Owner = Me}
        diag.Show()
    End Sub
#End Region
End Class
