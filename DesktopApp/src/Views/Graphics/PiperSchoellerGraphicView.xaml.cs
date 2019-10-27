using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Wells.CoreView;
using Wells.View.ViewModels;

namespace Wells.View
{
    /// <summary>
    /// Interaction logic for IPiperSchoellerGraphicView.xaml
    /// </summary>
    public partial class PiperSchoellerGraphicView : Window, IPiperSchoellerGraphicView
    {
        PiperSchoellerGraphicViewModel viewModel;
        public PiperSchoellerGraphicView(PiperSchoellerGraphicViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.viewModel.SetView(this);
            DataContext = this.viewModel;

            CalculateXYPositions();
            CreateGraphics();
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


        private void OnPointCheckedChanged(object sender, RoutedEventArgs e)
        {
            CreateGraphics();
        }

        public void CreateGraphics()
        {
            DrawPoints();
            CreateLegend();
        }

        void CalculateXYPositions()
        {
            var lowerLeftCations = new Point(4328.1299, 18544.4805);
            var lowerLeftAnions = new Point(16255.1201, 18544.4805);
            double triangleSideLength = 8544.5205;

            foreach (var p in viewModel.PiperSchollerPoints)
                p.CalculateXYPositions(lowerLeftCations, lowerLeftAnions, triangleSideLength);
        }

        void DrawPoints()
        {
            var baseDrawing = (Application.Current.FindResource("PiperImage") as DrawingImage).Clone();
            var dGroup = ((baseDrawing.Drawing as DrawingGroup).Children[0] as DrawingGroup);

            if (viewModel.ShowZones)
            {
                var zones = (Application.Current.FindResource("PiperZonesDrawingGroup") as DrawingGroup).Clone();
                dGroup.Children.Insert(0, zones.Children[0]);
                dGroup.Children.Insert(1, zones.Children[1]);
                dGroup.Children.Insert(2, zones.Children[2]);
                dGroup.Children.Insert(3, zones.Children[3]);
            }


            using (var dc = dGroup.Append())
            {
                foreach (var p in viewModel.PiperSchollerPoints.Where(point => point.IsVisible))
                {
                    var brush = new SolidColorBrush(p.PointColor);
                    var pen = new Pen(new SolidColorBrush(Colors.Black), 2);

                    dc.DrawEllipse(brush, pen, p.Cation, viewModel.PointSize, viewModel.PointSize);
                    dc.DrawEllipse(brush, pen, p.Anion, viewModel.PointSize, viewModel.PointSize);
                    dc.DrawEllipse(brush, pen, p.Diamond, viewModel.PointSize, viewModel.PointSize);
                }
            }

            if (baseDrawing.CanFreeze) baseDrawing.Freeze();
            PiperImage.Source = baseDrawing;
        }


        void CreateLegend()
        {
            LegendStackPanel.Children.Clear();

            foreach (var p in viewModel.PiperSchollerPoints.Where(point => point.IsVisible))
            {
                var ell = new Ellipse { Fill = new SolidColorBrush(p.PointColor), Width = 7, Height = 7, Margin = new Thickness(2, 1, 2, 1) };
                var text = new TextBlock() { Text = p.Label, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(2, 1, 2, 1) };
                var itemSP = new StackPanel() { Orientation = Orientation.Horizontal };
                itemSP.Children.Add(ell);
                itemSP.Children.Add(text);
                LegendStackPanel.Children.Add(itemSP);
            }
        }

        public Color ShowColorDialog(System.Drawing.Color selectedColor)
        {
            using (var diag = new System.Windows.Forms.ColorDialog() { Color = selectedColor, FullOpen = true })
            {
                if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    return Color.FromArgb(diag.Color.A, diag.Color.R, diag.Color.G, diag.Color.B);
            }
            return Color.FromArgb(selectedColor.A, selectedColor.R, selectedColor.G, selectedColor.B);
        }

        public void SaveImage(string filename = "")
        {
            var imageFilename = SaveFileDialog("Imagenes *.png|*.png", "Guardar imagen", filename);
            if (!string.IsNullOrEmpty(imageFilename))
                SharedBaseView.CaptureScreen(imageFilename, PiperCanvas, 200, 200);
        }
    }
}
