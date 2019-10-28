namespace Wells.View
{
    public interface IChartGraphicsView : IGraphicsView
    {
        int GetYAxisIndex(string axisTitle);
        void AddAxis(LiveCharts.Wpf.Axis axis);
        void RemoveAxis(int axisIndex);
        void ResetZoom();
    }
}
