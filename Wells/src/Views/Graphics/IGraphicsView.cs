using Wells.BaseView.ViewInterfaces;

namespace Wells.View
{
    public interface IGraphicsView : IView
    {
        void SaveImage(string filename = "");
    }
}
