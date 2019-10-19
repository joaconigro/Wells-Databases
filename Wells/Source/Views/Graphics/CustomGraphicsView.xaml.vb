Imports LiveCharts.Wpf
Imports Wells
Imports Wells.ViewBase

Public Class CustomGraphicsView
    Implements IChartGraphicsView

    Private _ViewModel As CustomGraphicsViewModel

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _ViewModel = New CustomGraphicsViewModel(Me)
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

    Public Function GetYAxisIndex(axisTitle As String) As Integer Implements IChartGraphicsView.GetYAxisIndex
        If MainChart.AxisY.ToList.Exists(Function(a) a.Title = axisTitle) Then
            Return MainChart.AxisY.ToList.FindIndex(Function(a) a.Title = axisTitle)
        Else
            If MainChart.AxisY.ToList.Count = 1 Then
                If String.IsNullOrEmpty(MainChart.AxisY(0).Title) Then
                    MainChart.AxisY(0).Title = axisTitle
                    Return 0
                End If
            End If
        End If
        Return -1
    End Function

    Public Sub AddAxis(axis As Axis) Implements IChartGraphicsView.AddAxis
        MainChart.AxisY.Add(axis)
    End Sub

    Public Sub ResetZoom() Implements IChartGraphicsView.ResetZoom
        X.MinValue = Double.NaN
        X.MaxValue = Double.NaN
        Y.MinValue = Double.NaN
        Y.MaxValue = Double.NaN
    End Sub

    Public Sub RemoveAxis(axisIndex As Integer) Implements IChartGraphicsView.RemoveAxis
        If axisIndex > -1 AndAlso axisIndex < MainChart.AxisY.Count Then
            MainChart.AxisY.RemoveAt(axisIndex)
        End If
        For Each axis In MainChart.AxisY
            axis.LabelFormatter = _ViewModel.YFormatter
            axis.FontSize = 12
        Next
    End Sub

    Public Sub SaveImage(Optional filename As String = "") Implements IGraphicsView.SaveImage
        Dim imageFilename = SaveFileDialog("Imagenes *.png|*.png", "Guardar imagen", filename)
        If Not String.IsNullOrEmpty(imageFilename) Then
            SharedBaseView.CaptureScreen(imageFilename, MainChart, 200, 200)
        End If
    End Sub

End Class
