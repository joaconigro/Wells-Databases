Imports LiveCharts.Wpf

Public Class GraphicsView
    Implements IGraphicsView

    Private _viewModel As GraphicsViewModel

    Sub New(viewmodel As GraphicsViewModel)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _viewModel = viewmodel
        _viewModel.View = Me
        DataContext = _viewModel
    End Sub

    Public Function ShowMessageBox(message As String, title As String) As Boolean Implements IView.ShowMessageBox
        Return SharedBaseView.ShowMessageBox(Me, message, title)
    End Function

    Public Sub ShowErrorMessageBox(message As String) Implements IView.ShowErrorMessageBox
        SharedBaseView.ShowErrorMessageBox(Me, message)
    End Sub

    Public Function OpenFileDialog(filter As String, title As String) As String Implements IView.OpenFileDialog
        Return String.Empty
    End Function

    Public Function SaveFileDialog(filter As String, title As String, Optional filename As String = "") As String Implements IView.SaveFileDialog
        Return String.Empty
    End Function

    Public Function GetYAxis() As List(Of Axis) Implements IGraphicsView.GetYAxis
        Return MainChart.AxisY.ToList
    End Function

    Public Function GetYAxis(axisTitle As String) As Integer Implements IGraphicsView.GetYAxis
        If MainChart.AxisY.ToList.Exists(Function(a) a.Title = axisTitle) Then
            Return MainChart.AxisY.ToList.FindIndex(Function(a) a.Title = axisTitle)
        End If
        Return -1
    End Function

    Public Sub AddAxis(axis As Axis) Implements IGraphicsView.AddAxis
        MainChart.AxisY.Add(axis)
    End Sub
End Class
