namespace Wells.View
{
    interface IMapView : IGraphicsView
    {
        void UpdateMap();
        void UpdateHeading(double heading);
        bool ShowManageColorMapDialog();
    }
}
