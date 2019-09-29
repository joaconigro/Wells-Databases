Imports Wells.ViewBase
Public Interface IGraphicsView
    Inherits IView

    Function GetYAxisIndex(axisTitle As String) As Integer
    Sub AddAxis(axis As LiveCharts.Wpf.Axis)
    Sub RemoveAxis(axisIndex As Integer)
    Sub ResetZoom()
    Sub SaveChartImage(Optional filename As String = "")
End Interface
