Imports LiveCharts.Wpf
Imports Wells
Imports Wells.ViewBase

Public Class CustomGraphicsView
    Implements IGraphicsView

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

    Public Function GetYAxisIndex(axisTitle As String) As Integer Implements IGraphicsView.GetYAxisIndex
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

    Public Sub AddAxis(axis As Axis) Implements IGraphicsView.AddAxis
        MainChart.AxisY.Add(axis)
    End Sub

    Public Sub ResetZoom() Implements IGraphicsView.ResetZoom
        X.MinValue = Double.NaN
        X.MaxValue = Double.NaN
        Y.MinValue = Double.NaN
        Y.MaxValue = Double.NaN
    End Sub

    Public Sub RemoveAxis(axisIndex As Integer) Implements IGraphicsView.RemoveAxis
        If axisIndex > -1 AndAlso axisIndex < MainChart.AxisY.Count Then
            MainChart.AxisY.RemoveAt(axisIndex)
        End If
        For Each axis In MainChart.AxisY
            axis.LabelFormatter = _ViewModel.YFormatter
            axis.FontSize = 12
        Next
    End Sub

    Public Sub SaveChartImage(Optional filename As String = "") Implements IGraphicsView.SaveChartImage
        Dim imageFilename = SaveFileDialog("Imagenes *.png|*.png", "Guardar imagen", filename)
        If Not String.IsNullOrEmpty(imageFilename) Then

            'Dim bitmap = New RenderTargetBitmap(MainChart.ActualWidth, MainChart.ActualHeight, 200, 200, PixelFormats.Pbgra32)
            'bitmap.Render(MainChart)
            Dim bms = CaptureScreen(MainChart, 200, 200)
            Dim frame = BitmapFrame.Create(bms)
            Dim encoder = New PngBitmapEncoder()
            encoder.Frames.Add(Frame)
            Using stream = IO.File.Create(imageFilename)
                encoder.Save(stream)
            End Using
        End If
    End Sub

    Private Function CaptureScreen(target As Visual, dpiX As Double, dpiY As Double) As BitmapSource
        If target Is Nothing Then
            Return Nothing
        End If

        Dim bounds = VisualTreeHelper.GetDescendantBounds(target)
        Dim rtb = New RenderTargetBitmap(CInt(bounds.Width * dpiX / 96.0),
                                         CInt(bounds.Height * dpiY / 96.0),
                                         dpiX, dpiY, PixelFormats.Pbgra32)

        Dim dv As New DrawingVisual()
        Using ctx = dv.RenderOpen()
            Dim vb = New VisualBrush(target)
            ctx.DrawRectangle(vb, Nothing, New Rect(New Point(), bounds.Size))
        End Using

        rtb.Render(dv)
        Return rtb
    End Function

End Class
