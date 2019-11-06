namespace Wells.BaseView.ViewInterfaces
{
    public interface IView
    {
        void CloseView(bool? dialogResult);
        void CloseView();
    }
}
