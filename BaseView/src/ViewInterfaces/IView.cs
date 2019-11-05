namespace Wells.BaseView.ViewInterfaces
{
    public interface IView
    {
        bool ShowYesNoMessageBox(string message, string title);
        void ShowErrorMessageBox(string message);
        void ShowOkOnkyMessageBox(string message, string title);
        void CloseView(bool? dialogResult);
        void CloseView();
    }
}
