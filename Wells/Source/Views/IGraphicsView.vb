Imports Wells.ViewBase
Public Interface IGraphicsView
    Inherits IView

    Function GetYAxis() As List(Of LiveCharts.Wpf.Axis)
    Function GetYAxis(axisTitle As String) As Integer
    Sub AddAxis(axis As LiveCharts.Wpf.Axis)
End Interface
