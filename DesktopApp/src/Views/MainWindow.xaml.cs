using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Wells.CoreView;
using Wells.CoreView.ViewInterfaces;
using Wells.View.Graphics;
using Wells.View.ViewModels;
using Wells.YPFModel;

namespace Wells.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindow
    {
        MainWindowViewModel viewModel;
        private WaitingView waitingDialog;

        public event EventHandler PremadeGraphicsChanged;

        public MainWindow()
        {
            InitializeComponent();

            viewModel = new MainWindowViewModel(this);
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

        public string OpenFileDialog(string filter, string title, string initialDirectory = "")
        {
            return SharedBaseView.OpenFileDialog(filter, title, initialDirectory);
        }

        public string SaveFileDialog(string filter, string title, string filename, string initialDirectory = "")
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

        private void AfterContentRendered(object sender, System.EventArgs e)
        {
            string connectionString = Application.Current.Resources["CILPConnection"].ToString();
            if (!string.IsNullOrEmpty(connectionString))
            {
                Task.Run(() =>
                {
                    viewModel.InitializeContext(connectionString);
                    CreateViewModelsForPanels();
                    CloseWaitingMessage();
                });
            }
        }

        private void CreateViewModelsForPanels()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                WellsControl.SetViewModel(new WellsViewModel());
                MeasurementsControl.SetViewModel(new MeasurementsViewModel());
                WaterAnalysesControl.SetViewModel(new WaterAnalysesViewModel());
                SoilAnalysesControl.SetViewModel(new SoilAnalysesViewModel());
                FLNAAnalysesControl.SetViewModel(new FLNAAnalysesViewModel());
                PrecipitationsControl.SetViewModel(new PrecipitationsViewModel());
            });
        }

        public int SelectSheetDialog(List<string> sheets)
        {
            var diag = new ExcelSheetsView(sheets) { Owner = this };
            if ((bool)diag.ShowDialog())
                return diag.SelectedSheet;
            return -1;
        }

        public void ShowWaitingMessage(string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                IsEnabled = false;
                waitingDialog = new WaitingView(message) { Owner = this };
                waitingDialog.Show();
            });
        }

        public void CloseWaitingMessage()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                waitingDialog?.Close();
                waitingDialog = null;
                IsEnabled = true;
            });
        }

        public void OpenGraphicsView()
        {
            var diag = new CustomGraphicsView();
            diag.Show();
        }

        public void OpenGraphicsView(Well well, PremadeSeriesInfoCollection series)
        {
            var diag = new PremadeGraphicsView(well, series);
            diag.Show();
        }

        public void OpenGraphicsView(PiperSchoellerGraphicViewModel viewModel)
        {
            var diag = new PiperSchoellerGraphicView(viewModel);
            diag.Show();
        }

        public void OpenCreatePremadeGraphicView()
        {
            var diag = new CreatePremadeGraphicView();
            diag.Show();
            PremadeGraphicsChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool OpenEditEntityDialog(EditWellViewModel viewModel)
        {
            var diag = new EditWellView(viewModel);
            return (bool)diag.ShowDialog();
        }

        public bool OpenEditEntityDialog(EditMeasurementViewModel viewModel)
        {
            var diag = new EditMeasurementView(viewModel);
            return (bool)diag.ShowDialog();
        }
    }

    public interface IMainWindow : IView
    {
        int SelectSheetDialog(List<string> sheets);
        void ShowWaitingMessage(string message);
        void CloseWaitingMessage();
        void OpenGraphicsView();
        void OpenGraphicsView(YPFModel.Well well, PremadeSeriesInfoCollection series);
        void OpenGraphicsView(PiperSchoellerGraphicViewModel viewModel);
        void OpenCreatePremadeGraphicView();
        bool OpenEditEntityDialog(EditWellViewModel viewModel);
        bool OpenEditEntityDialog(EditMeasurementViewModel viewModel);

        event EventHandler PremadeGraphicsChanged;
    }
}
