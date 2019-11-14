using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Wells.BaseView.ViewInterfaces;
using Wells.Model;
using Wells.View.Graphics;
using Wells.View.ViewModels;

namespace Wells.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindow
    {
        readonly MainWindowViewModel viewModel;
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
        #endregion

        private void AfterContentRendered(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                viewModel.InitializeContext();
                CreateViewModelsForPanels();
                CloseWaitingMessage();
            });
        }

        private void CreateViewModelsForPanels()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                WellsControl.SetViewModel(new WellsViewModel());
                MeasurementsControl.SetViewModel(new MeasurementsViewModel());
                WaterAnalysesControl.SetViewModel(new WaterAnalysesViewModel());
                SoilAnalysesControl.SetViewModel(new SoilAnalysesViewModel());
                FLNAAnalysesControl.SetViewModel(new FlnaAnalysesViewModel());
                PrecipitationsControl.SetViewModel(new PrecipitationsViewModel());
            });
        }

        public int SelectSheetDialog(List<string> sheets)
        {
            return Application.Current.Dispatcher.Invoke(() =>
            {
                var diag = new ExcelSheetsView(sheets) { Owner = this };
                if ((bool)diag.ShowDialog()) { return diag.SelectedSheet; };
                return -1;
            });
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
            var diag = new CustomGraphicsView { Owner = this };
            diag.Show();
        }

        public void OpenGraphicsView(Well well, PremadeSeriesInfoCollection series)
        {
            var diag = new PremadeGraphicsView(well, series) { Owner = this };
            diag.Show();
        }

        public void OpenGraphicsView(PiperSchoellerGraphicViewModel viewModel)
        {
            var diag = new PiperSchoellerGraphicView(viewModel) { Owner = this };
            diag.Show();
        }

        public void OpenGraphicsView(MapViewModel viewModel)
        {
            var diag = new MapView(viewModel) { Owner = this };
            diag.Show();
        }

        public void OpenCreatePremadeGraphicView()
        {
            var diag = new CreatePremadeGraphicView { Owner = this };
            diag.ShowDialog();
            PremadeGraphicsChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool OpenEditEntityDialog(EditWellViewModel viewModel)
        {
            var diag = new EditWellView(viewModel) { Owner = this };
            return (bool)diag.ShowDialog();
        }

        public bool OpenEditEntityDialog(EditMeasurementViewModel viewModel)
        {
            var diag = new EditMeasurementView(viewModel) { Owner = this };
            return (bool)diag.ShowDialog();
        }

        public void ShowManageColorMapDialog()
        {
            var diag = new ManageColorMapsView { Owner = this };
            diag.ShowDialog();
        }
    }

    public interface IMainWindow : IView
    {
        int SelectSheetDialog(List<string> sheets);
        void ShowWaitingMessage(string message);
        void CloseWaitingMessage();
        void OpenGraphicsView();
        void OpenGraphicsView(Well well, PremadeSeriesInfoCollection series);
        void OpenGraphicsView(PiperSchoellerGraphicViewModel viewModel);
        void OpenGraphicsView(MapViewModel viewModel);
        void OpenCreatePremadeGraphicView();
        void ShowManageColorMapDialog();
        bool OpenEditEntityDialog(EditWellViewModel viewModel);
        bool OpenEditEntityDialog(EditMeasurementViewModel viewModel);

        event EventHandler PremadeGraphicsChanged;
    }
}
