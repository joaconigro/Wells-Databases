using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;
using Wells.BaseView.ViewInterfaces;
using Wells.View.Graphics;
using Wells.View.ViewModels;
using System.Windows.Controls;
using Wells.View.UserControls;

namespace Wells.View
{
    /// <summary>
    /// Interaction logic for ManageColorMapsView.xaml
    /// </summary>
    public partial class ManageColorMapsView : Window, IManageColorMapsView
    {
        private readonly ManageColorMapsViewModel viewModel;
        private readonly List<ColorSlider> sliders;
        private ColorSlider selectedSlider;
        public ManageColorMapsView()
        {
            InitializeComponent();
            sliders = new List<ColorSlider>();
            viewModel = new ManageColorMapsViewModel(this);
            DataContext = viewModel;

            ColorWheel.ColorChanged += ColorWheelColorChanged;
            ColorEditor.ColorChanged += ColorEditorColorChanged;
        }

        private void ColorEditorColorChanged(object sender, System.EventArgs e)
        {
            ColorWheel.Color = ColorEditor.Color;
            if (selectedSlider != null) { selectedSlider.DrawingColor = ColorEditor.Color; }
        }

        private void ColorWheelColorChanged(object sender, System.EventArgs e)
        {
            ColorEditor.Color = ColorWheel.Color;
            if (selectedSlider != null) { selectedSlider.DrawingColor = ColorWheel.Color; }
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


        public void CreateSliders()
        {
            sliders.Clear();
            SlidersCanvas.Children.Clear();

            foreach (var g in viewModel.SelectedGradient.LinearGradient.GradientStops)
            {
                var s = new ColorSlider(SlidersCanvas, ScaleRectangle, g);
                s.MouseLeftButtonDown += ColorSliderMouseLeftButtonDown;
                sliders.Add(s);
                Canvas.SetLeft(s, s.CanvasLeftPosition);
                SlidersCanvas.Children.Add(s);
            }

            
        }

        private void ColorSliderMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            selectedSlider = sender as ColorSlider;
            var color = selectedSlider.DrawingColor;
            ColorWheel.Color = color;
            ColorEditor.Color = color;
        }

        void ClearSliders()
        {

        }
    }

    public interface IManageColorMapsView : IView
    {
        void CreateSliders();
    }
}
