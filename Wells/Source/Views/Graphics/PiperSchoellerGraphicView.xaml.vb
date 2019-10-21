Imports System.Drawing
Imports Wells
Imports Wells.ViewBase

Public Class PiperSchoellerGraphicView
    Implements IPiperSchoellerGraphicView

    Private _ViewModel As PiperSchoellerGraphicViewModel

    Sub New(viewModel As PiperSchoellerGraphicViewModel)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _ViewModel = viewModel
        _ViewModel.SetView(Me)
        DataContext = _ViewModel

        CalculateXYPositions()
        CreateGraphics()
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

    Public Sub SaveImage(Optional filename As String = "") Implements IGraphicsView.SaveImage
        Dim imageFilename = SaveFileDialog("Imagenes *.png|*.png", "Guardar imagen", filename)
        If Not String.IsNullOrEmpty(imageFilename) Then
            SharedBaseView.CaptureScreen(imageFilename, PiperCanvas, 200, 200)
        End If
    End Sub

    Private Sub CalculateXYPositions()
        Dim lowerLeftCations As New Windows.Point(4328.1299, 18544.4805)
        Dim lowerLeftAnions As New Windows.Point(16255.1201, 18544.4805)
        Dim triangleSideLength As Double = 8544.5205

        For Each p In _ViewModel.PiperSchollerPoints
            p.CalculateXYPositions(lowerLeftCations, lowerLeftAnions, triangleSideLength)
        Next
    End Sub

    Private Sub CreateGraphics() Implements IPiperSchoellerGraphicView.CreateGraphics
        DrawPoints()
        CreateLegend()
    End Sub

    Private Sub DrawPoints()
        Dim baseDrawing = CType(Application.Current.FindResource("PiperImage"), DrawingImage).Clone
        Dim dGroup = CType(CType(baseDrawing.Drawing, DrawingGroup).Children(0), DrawingGroup)

        If _ViewModel.ShowZones Then
            Dim zones = CType(Application.Current.FindResource("PiperZonesDrawingGroup"), DrawingGroup).Clone
            dGroup.Children.Insert(0, zones.Children.Item(0))
            dGroup.Children.Insert(1, zones.Children.Item(1))
            dGroup.Children.Insert(2, zones.Children.Item(2))
            dGroup.Children.Insert(3, zones.Children.Item(3))
        End If


        Using dc = dGroup.Append()
            For Each p In _ViewModel.PiperSchollerPoints.Where(Function(point) point.IsVisible)
                Dim brush = New SolidColorBrush(p.PointColor)
                Dim pen = New Media.Pen(New SolidColorBrush(Colors.Black), 2)

                dc.DrawEllipse(brush, pen, p.Cation, _ViewModel.PointSize, _ViewModel.PointSize)
                dc.DrawEllipse(brush, pen, p.Anion, _ViewModel.PointSize, _ViewModel.PointSize)
                dc.DrawEllipse(brush, pen, p.Diamond, _ViewModel.PointSize, _ViewModel.PointSize)
            Next
        End Using

        If baseDrawing.CanFreeze Then baseDrawing.Freeze()
        PiperImage.Source = baseDrawing
    End Sub


    Private Sub CreateLegend()
        LegendStackPanel.Children.Clear()

        For Each p In _ViewModel.PiperSchollerPoints.Where(Function(point) point.IsVisible)
            Dim ell As New Ellipse With {.Fill = New SolidColorBrush(p.PointColor), .Width = 7, .Height = 7, .Margin = New Thickness(2, 1, 2, 1)}
            Dim text As New TextBlock() With {.Text = p.Label, .VerticalAlignment = VerticalAlignment.Center, .Margin = New Thickness(2, 1, 2, 1)}
            Dim itemSP As New StackPanel() With {.Orientation = Orientation.Horizontal}
            itemSP.Children.Add(ell)
            itemSP.Children.Add(text)
            LegendStackPanel.Children.Add(itemSP)
        Next
    End Sub

    Function ShowColorDialog(selectedColor As Color) As Media.Color Implements IPiperSchoellerGraphicView.ShowColorDialog
        Using diag As New Forms.ColorDialog() With {.Color = selectedColor, .FullOpen = True}
            If diag.ShowDialog = Forms.DialogResult.OK Then
                Return Media.Color.FromArgb(diag.Color.A, diag.Color.R, diag.Color.G, diag.Color.B)
            End If
        End Using
        Return Media.Color.FromArgb(selectedColor.A, selectedColor.R, selectedColor.G, selectedColor.B)
    End Function

    Private Sub OnPointCheckedChanged(sender As Object, e As RoutedEventArgs)
        CreateGraphics()
    End Sub
End Class

Public Interface IPiperSchoellerGraphicView
    Inherits IGraphicsView

    Sub CreateGraphics()
    Function ShowColorDialog(selectedColor As Color) As Media.Color
End Interface
