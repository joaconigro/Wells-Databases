Imports Wells.ViewBase
Public Interface IGraphicsView
    Inherits IView

    Function GetYAxisIndex(axisTitle As String) As Integer
    Sub AddAxis(axis As LiveCharts.Wpf.Axis)
End Interface
