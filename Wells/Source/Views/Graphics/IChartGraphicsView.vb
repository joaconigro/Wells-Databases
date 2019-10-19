Imports Wells.ViewBase
Public Interface IChartGraphicsView
    Inherits IGraphicsView

    Function GetYAxisIndex(axisTitle As String) As Integer
    Sub AddAxis(axis As LiveCharts.Wpf.Axis)
    Sub RemoveAxis(axisIndex As Integer)
    Sub ResetZoom()
End Interface
