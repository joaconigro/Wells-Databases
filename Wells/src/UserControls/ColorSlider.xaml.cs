using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Wells.View.Graphics;

namespace Wells.View.UserControls
{
    /// <summary>
    /// Interaction logic for ColorSlider.xaml
    /// </summary>
    public partial class ColorSlider : UserControl
    {
        private System.Drawing.Color drawingColor;
        private Color mediaColor;
        private readonly Canvas canvas;
        private double gradientOffset;
        private double canvasLeftPosition;
        private bool _isDragging;
        private Point _offset;
        private bool isSelected;

        public GradientStop GradientStop { get; }
        public bool CanDrag { get; }
        public SolidColorBrush SliderFill { get => (SolidColorBrush)GetValue(SliderFillProperty); set => SetValue(SliderFillProperty, value); }
        public bool IsSelected 
        { 
            get => isSelected;
            set 
            { 
                isSelected = value;
                SliderFill = isSelected ? new SolidColorBrush(Colors.DeepSkyBlue) : new SolidColorBrush(Colors.LightGray);
            }
        }

        public static readonly DependencyProperty SliderFillProperty = DependencyProperty.Register(nameof(SliderFill),
         typeof(SolidColorBrush), typeof(ColorSlider), new PropertyMetadata(new SolidColorBrush(Colors.LightGray)));

        public ColorSlider(Canvas canvas, GradientStop gradientStop)
        {
            InitializeComponent();
            DataContext = this;

            this.canvas = canvas;
            GradientStop = gradientStop;

            GradientOffset = GradientStop.Offset;
            MediaColor = GradientStop.Color;

            CanDrag = !(GradientOffset.Equals(0.0) || GradientOffset.Equals(1.0));
            SliderFill = new SolidColorBrush(Colors.LightGray);
        }


        public double GradientOffset { get => gradientOffset; set { gradientOffset = value; UpdateCanvasLeft(); } }
        public double CanvasLeftPosition
        {
            get => canvasLeftPosition;
            set
            {
                canvasLeftPosition = value;
                UpdateGradientOffset();
                GradientStop.Offset = gradientOffset;
            }
        }

        public System.Drawing.Color DrawingColor
        {
            get => drawingColor;
            set
            {
                drawingColor = value;
                mediaColor = drawingColor.ToMediaColor();
                GradientStop.Color = mediaColor;
            }
        }

        public Color MediaColor { get => mediaColor; set { mediaColor = value; drawingColor = mediaColor.ToDrawingColor(); } }

        void UpdateGradientOffset()
        {
            gradientOffset = (canvasLeftPosition + 4.0) / canvas.ActualWidth;
        }

        void UpdateCanvasLeft()
        {
            canvasLeftPosition = canvas.ActualWidth * gradientOffset - 4.0;
        }


        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (CanDrag)
            {
                _isDragging = true;
                _offset = e.GetPosition(this);
            }
        }

        private void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (CanDrag)
            {
                // If we're not dragging, don't bother - also validate the element
                if (!_isDragging) { return; }

                // Get the position of the mouse relative to the canvas
                Point mousePoint = e.GetPosition(canvas);

                // Offset the mouse position by the original offset position
                mousePoint.Offset(-_offset.X, -_offset.Y);

                // Move the element on the canvas
                var value = mousePoint.X;
                if (value < -4.0) { value = -4.0; }
                else if (value > canvas.ActualWidth + 4.0) { value = canvas.ActualWidth + 4.0; }
                this.SetValue(Canvas.LeftProperty, value);
                CanvasLeftPosition = value;
            }
        }

        private void OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDragging = false;
        }

    }

}
