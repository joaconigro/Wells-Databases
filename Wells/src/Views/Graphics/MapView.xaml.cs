using Microsoft.Maps.MapControl.WPF;
using System;
using System.Linq;
using System.Windows;
using Wells.BaseView;
using Wells.View.ViewModels;

namespace Wells.View
{
    /// <summary>
    /// Interaction logic for MapView.xaml
    /// </summary>
    public partial class MapView : Window, IMapView
    {
        readonly MapViewModel viewModel;
        public MapView(MapViewModel vm)
        {
            InitializeComponent();
            viewModel = vm;
            viewModel.SetView(this);
            DataContext = viewModel;
            Map.ViewChangeOnFrame += OnMapViewChangeOnFrame;
        }

        private void OnMapViewChangeOnFrame(object sender, MapEventArgs e)
        {
            Map.ZoomLevel = Math.Min(Map.ZoomLevel, 19.0);
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


        public void UpdateMap()
        {
            if (viewModel.Pushpins.Any())
            {
                Map.Children.Clear();
                foreach (var p in viewModel.Pushpins)
                {
                    Map.Children.Add(p);
                }
                var locs = viewModel.Pushpins.Select(p => p.Location);
                Map.SetView(locs, new Thickness(10), viewModel.MapRotation, 18);
            }
        }

        public void UpdateHeading(double heading)
        {
            Map.Heading = heading;
        }

        public void SaveImage(string filename)
        {
            var imageFilename = SharedBaseView.SaveFileDialog("Imagenes *.png|*.png", "Guardar imagen", filename);
            if (!string.IsNullOrEmpty(imageFilename))
            {
                SharedBaseView.CaptureScreen(imageFilename, Map, 200, 200);
            }
        }

        private void OnMapMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.RightButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                viewModel.ClearSelection();
            }
        }

        public bool ShowManageColorMapDialog()
        {
            var diag = new ManageColorMapsView { Owner = this };
            return (bool)diag.ShowDialog();
        }

        private void AfterContentRendered(object sender, System.EventArgs e)
        {
            UpdateMap();
        }
    }
}
