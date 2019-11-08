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
        private GradientStop gradientStop;
        private double gradientOffset;
        private double canvasLeftPosition;
        private bool _isDragging;
        private readonly bool canDrag;
        private Point _offset;
        private readonly double rectWidth;
        private readonly double traslation;

        public ColorSlider(Canvas canvas, Rectangle rectangle, GradientStop gradientStop)
        {
            InitializeComponent();

            rectWidth = rectangle.ActualWidth;
            traslation = (rectWidth - canvas.ActualWidth) / 2.0;
            this.canvas = canvas;
            this.gradientStop = gradientStop;

            GradientOffset = this.gradientStop.Offset;
            MediaColor = this.gradientStop.Color;

            canDrag = !GradientOffset.Equals(0.0) & !GradientOffset.Equals(1.0);
        }


        public double GradientOffset { get => gradientOffset; set { gradientOffset = value; UpdateCanvasLeft(); } }
        public double CanvasLeftPosition
        {
            get => canvasLeftPosition;
            set
            {
                if (value < 0.0) { value = 0.0; }
                else if (value > rectWidth) { value = rectWidth; }
                canvasLeftPosition = value;
                UpdateGradientOffset();
                gradientStop.Offset = gradientOffset;
            }
        }

        public System.Drawing.Color DrawingColor
        {
            get => drawingColor;
            set
            {
                drawingColor = value;
                mediaColor = drawingColor.ToMediaColor();
                gradientStop.Color = mediaColor;
            }
        }

        public Color MediaColor { get => mediaColor; set { mediaColor = value; drawingColor = mediaColor.ToDrawingColor(); } }

        void UpdateGradientOffset()
        {
            if (canvasLeftPosition.Equals(0.0))
            {
                gradientOffset = 0.0;
            }
            else
            {
                gradientOffset = canvasLeftPosition / rectWidth;
            }
        }

        void UpdateCanvasLeft()
        {
            if (gradientOffset.Equals(0.0))
            {
                canvasLeftPosition = 0.0;
            }
            else
            {
                canvasLeftPosition = rectWidth * gradientOffset;
            }
        }


        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (canDrag)
            {
                _isDragging = true;
                _offset = e.GetPosition(this);
            }
        }

        private void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (canDrag)
            {
                // If we're not dragging, don't bother - also validate the element
                if (!_isDragging) return;

                // Get the position of the mouse relative to the canvas
                Point mousePoint = e.GetPosition(canvas);

                // Offset the mouse position by the original offset position
                mousePoint.Offset(-_offset.X, -_offset.Y);

                // Move the element on the canvas
                var value = mousePoint.X + traslation;
                if (value < 0.0) { value = 0.0; }
                else if (value > rectWidth) { value = rectWidth; }
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
