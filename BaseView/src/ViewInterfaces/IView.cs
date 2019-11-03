namespace Wells.BaseView.ViewInterfaces
{
    public interface IView
    {
        string OpenFileDialog(string filter, string title);
        string OpenFileDialog(string filter, string title, string initialDirectory);
        bool ShowYesNoMessageBox(string message, string title);
        void ShowErrorMessageBox(string message);
        void ShowOkOnkyMessageBox(string message, string title);
        string SaveFileDialog(string filter, string title);
        string SaveFileDialog(string filter, string title, string filename);
        string SaveFileDialog(string filter, string title, string filename, string initialDirectory);
        void CloseView(bool? dialogResult);
        void CloseView();
    }
}
