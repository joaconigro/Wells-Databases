using Wells.CoreView;
using Wells.CoreView.ViewInterfaces;
using LaTeAndes.DesktopCore.ViewModels;
using System.Windows;
using System.Collections.Generic;
using Wells.View.Graphics;

namespace Wells.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindow
    {
        MainWindowViewModel _ViewModel;

        public MainWindow()
        {
            InitializeComponent();

            _ViewModel = new MainWindowViewModel(this);
            DataContext = _ViewModel;

            SampleCollectionsControl.SetViewModel(new SamplesCollectionsViewModel());
            SamplesControl.SetViewModel(new SamplesViewModel());
            ClientUsersControl.SetViewModel(new ClientUsersViewModel());
            ClientsControl.SetViewModel(new ClientsViewModel());
            AvailableStudiesControl.SetViewModel(new AvailableStudiesViewModel());
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

        public string SaveFileDialog(string filter, string title, string filename, string initialDirectory)
        {
            return SharedBaseView.SaveFileDialog(filter, title, filename, initialDirectory);
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


        public bool OpenEditEntityDialog(ClientViewModel vm)
        {
            var diag = new ClientView(vm) { Owner = this };
            return (bool)diag.ShowDialog();
        }

        public bool OpenEditEntityDialog(AvailableStudyViewModel vm)
        {
            var diag = new AvailableStudyView(vm) { Owner = this };
            return (bool)diag.ShowDialog();
        }

    }

    public interface IMainWindow : IView
    {
        bool CreateDatabaseDialog(ref string databaseName);
        int SelectSheetDialog(List<string> sheets);
    void ShowWaitingMessage(string message);
    void CloseWaitingMessage();
    void OpenGraphicsView();
    void OpenGraphicsView(YPFModel.Well well, PremadeSeriesInfoCollection series);
        void OpenGraphicsView(PiperSchoellerGraphicViewModel viewModel);
        void OpenCreatePremadeGraphicView();
        bool OpenEditEntityDialog(EditWellViewModel viewModel);
        bool OpenEditEntityDialog(EditMeasurementViewModel viewModel);

    delegate void PremadeGraphicsChanged();


    }
}
