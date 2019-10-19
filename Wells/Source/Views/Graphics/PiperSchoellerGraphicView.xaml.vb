Imports System.Drawing
Imports Wells
Imports Wells.ViewBase

Public Class PiperSchoellerGraphicView
    Implements IGraphicsView

    Private _ViewModel As PiperSchoellerGraphicViewModel
    Private _PiperSchollerPoints As List(Of PiperSchollerData)

    Sub New(viewModel As PiperSchoellerGraphicViewModel)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _ViewModel = viewModel
        _ViewModel.SetView(Me)
        _PiperSchollerPoints = _ViewModel.PiperSchollerPoints
        DataContext = _ViewModel

        CalculateXYPositions()
        DrawPoints()
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
            SharedBaseView.CaptureScreen(imageFilename, PiperImage, 200, 200)
        End If
    End Sub

    Private Sub CalculateXYPositions()
        Dim lowerLeftCations As New Windows.Point(4328.1299, 18544.4805)
        Dim lowerLeftAnions As New Windows.Point(16255.1201, 18544.4805)
        Dim triangleSideLength As Double = 8544.5205

        For Each p In _PiperSchollerPoints
            p.CalculateXYPositions(lowerLeftCations, lowerLeftAnions, triangleSideLength)
        Next
    End Sub

    Private Sub DrawPoints()
        Dim baseDrawing = CType(Application.Current.FindResource("PiperImage"), DrawingImage).Clone
        Dim dGroup = CType(CType(baseDrawing.Drawing, DrawingGroup).Children(0), DrawingGroup)
        Using dc = dGroup.Append()
            For Each p In _PiperSchollerPoints
                Dim brush = New SolidColorBrush(Colors.Red)
                Dim pen = New Media.Pen(New SolidColorBrush(Colors.Black), 1)
                'Dim arcSegment As New ArcSegment(p.Cation, New Windows.Size(50, 50), 360, True, SweepDirection.Clockwise, True)
                'Dim fig As New PathFigure()
                'Dim geom = New PathGeometry({arcSegment}, FillRule.EvenOdd, Nothing)

                dc.DrawEllipse(brush, pen, p.Cation, 100, 100)
                dc.DrawEllipse(brush, pen, p.Anion, 100, 100)
                dc.DrawEllipse(brush, pen, p.Diamond, 100, 100)
            Next
        End Using

        'CType(CType(baseDrawing.Drawing, DrawingGroup).Children(0), DrawingGroup).a.Children.Add(dGroup)
        If baseDrawing.CanFreeze Then baseDrawing.Freeze()
        PiperImage.Source = baseDrawing
    End Sub

End Class
