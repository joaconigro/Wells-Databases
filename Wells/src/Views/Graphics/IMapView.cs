namespace Wells.View
{
    interface IMapView : IGraphicsView
    {
        void UpdateMap();
        bool ShowManageColorMapDialog();
    }
}
