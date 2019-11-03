using System.Windows;
using Wells.BaseView;
using Wells.BaseView.ViewInterfaces;
using Wells.View.ViewModels;

namespace Wells.View
{
    /// <summary>
    /// Interaction logic for EditMeasurementView.xaml
    /// </summary>
    public partial class EditMeasurementView : Window, IView
    {
        readonly EditMeasurementViewModel viewModel;
        public EditMeasurementView(EditMeasurementViewModel vm)
        {
            InitializeComponent();

            viewModel = vm;
            viewModel.SetView(this);
            DataContext = viewModel;
        }

        #region IView
        public void CloseView(bool? dialogResult)
        {
            DialogResult = dialogResult;
            Close();
        }

        public void CloseView()
        {
            Close();
        }

        public string OpenFileDialog(string filter, string title, string initialDirectory)
        {
            return SharedBaseView.OpenFileDialog(filter, title, initialDirectory);
        }

        public string OpenFileDialog(string filter, string title)
        {
            return SharedBaseView.OpenFileDialog(filter, title);
        }

        public string SaveFileDialog(string filter, string title, string filename, string initialDirectory = "")
        {
            return SharedBaseView.SaveFileDialog(filter, title, filename, initialDirectory);
        }

        public string SaveFileDialog(string filter, string title)
        {
            return SharedBaseView.SaveFileDialog(filter, title);
        }

        public string SaveFileDialog(string filter, string title, string filename)
        {
            return SharedBaseView.SaveFileDialog(filter, title, filename);
        }

        public void ShowErrorMessageBox(string message)
        {
            SharedBaseView.ShowErrorMessageBox(this, message);
        }

        public bool ShowYesNoMessageBox(string message, string title)
        {
            return SharedBaseView.ShowYesNoMessageBox(this, message, title);
        }

        public void ShowOkOnkyMessageBox(string message, string title)
        {
            SharedBaseView.ShowOkOnkyMessageBox(this, message, title);
        }
        #endregion

    }
}
