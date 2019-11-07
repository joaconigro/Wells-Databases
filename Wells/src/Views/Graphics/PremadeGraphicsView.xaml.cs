using LiveCharts.Wpf;
using System.Linq;
using System.Windows;
using Wells.BaseView;
using Wells.View.Graphics;
using Wells.View.ViewModels;

namespace Wells.View
{
    /// <summary>
    /// Interaction logic for PremadeGraphicsView.xaml
    /// </summary>
    public partial class PremadeGraphicsView : Window, IChartGraphicsView
    {
        readonly PremadeGraphicsViewModel viewModel;
        public PremadeGraphicsView(Model.Well well, PremadeSeriesInfoCollection series)
        {
            InitializeComponent();

            viewModel = new PremadeGraphicsViewModel(this, well, series);
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


        public void AddAxis(Axis axis)
        {
            MainChart.AxisY.Add(axis);
        }


        public int GetYAxisIndex(string axisTitle)
        {
            if (MainChart.AxisY.ToList().Exists(a => a.Title == axisTitle))
            {
                return MainChart.AxisY.ToList().FindIndex(a => a.Title == axisTitle);
            }
            else
            {
                if (MainChart.AxisY.ToList().Count == 1)
                {
                    if (string.IsNullOrEmpty(MainChart.AxisY[0].Title))
                    {
                        MainChart.AxisY[0].Title = axisTitle;
                        return 0;
                    }
                }
            }
            return -1;
        }


        public void RemoveAxis(int axisIndex)
        {
            if (axisIndex > -1 && axisIndex < MainChart.AxisY.Count)
            {
                MainChart.AxisY.RemoveAt(axisIndex);
            }

            foreach (var axis in MainChart.AxisY)
            {
                axis.LabelFormatter = viewModel.YFormatter;
                axis.FontSize = 12;
            }
        }

        public void ResetZoom()
        {
            X.MinValue = double.NaN;
            X.MaxValue = double.NaN;
            Y.MinValue = double.NaN;
            Y.MaxValue = double.NaN;
        }

        public void SaveImage(string filename)
        {
            var imageFilename = SharedBaseView.SaveFileDialog("Imagenes *.png|*.png", "Guardar imagen", filename);
            if (!string.IsNullOrEmpty(imageFilename))
            {
                SharedBaseView.CaptureScreen(imageFilename, MainChart, 200, 200);
            }
        }
    }
}
