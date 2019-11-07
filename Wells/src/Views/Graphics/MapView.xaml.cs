using Microsoft.Maps.MapControl.WPF;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

            UpdateMap();
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
            Map.Children.Clear();
            foreach (var p in viewModel.Pushpins)
            {
                Map.Children.Add(p);
            }
         
            Map.Center = viewModel.CenterLocation;
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
    }
}
