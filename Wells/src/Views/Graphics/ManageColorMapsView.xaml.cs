using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;
using Wells.BaseView.ViewInterfaces;
using Wells.View.Graphics;
using Wells.View.ViewModels;
using System.Windows.Controls;
using Wells.View.UserControls;
using System.Windows.Input;

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
            ClearSliders();

            if (viewModel.SelectedGradient != null)
            {
                foreach (var g in viewModel.SelectedGradient.LinearGradient.GradientStops)
                {
                    var s = new ColorSlider(SlidersCanvas, g);
                    s.MouseLeftButtonDown += ColorSliderMouseLeftButtonDown;
                    s.MouseRightButtonDown += ColorSliderMouseRightButtonDown;
                    sliders.Add(s);
                    Canvas.SetLeft(s, s.CanvasLeftPosition);
                    SlidersCanvas.Children.Add(s);
                }
            }
        }

        private void ColorSliderMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ColorSlider slider && slider.CanDrag)
            {
                slider.MouseLeftButtonDown -= ColorSliderMouseLeftButtonDown;
                slider.MouseRightButtonDown -= ColorSliderMouseRightButtonDown;
                sliders.Remove(slider);
                SlidersCanvas.Children.Remove(slider);
                viewModel.SelectedGradient.LinearGradient.GradientStops.Remove(slider.GradientStop);
                e.Handled = true;
            }
        }

        private void ColorSliderMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ColorSlider slider)
            {
                selectedSlider = slider;
                var color = selectedSlider.DrawingColor;
                ColorWheel.Color = color;
                ColorEditor.Color = color;

                foreach(var s in sliders)
                {
                    s.IsSelected = false;
                }
                slider.IsSelected = true;

                e.Handled = true;
            }
        }

        void ClearSliders()
        {
            foreach(var s in sliders)
            {
                s.MouseLeftButtonDown -= ColorSliderMouseLeftButtonDown;
                s.MouseRightButtonDown -= ColorSliderMouseRightButtonDown;
            }
            sliders.Clear();
            SlidersCanvas.Children.Clear();
        }

        private void OnCanvasMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                if (sender is Canvas)
                {
                    var point = e.GetPosition(SlidersCanvas);
                    var offset = (point.X + 4.0) / SlidersCanvas.ActualWidth;
                    viewModel.SelectedGradient.AddGradientStop(offset);
                    viewModel.UpdateGradient();
                    e.Handled = true;
                }            
            }          
        }
    }

    public interface IManageColorMapsView : IView
    {
        void CreateSliders();
    }
}
