using System.Windows;

namespace Wells.View
{
    /// <summary>
    /// Interaction logic for WaitingView.xaml
    /// </summary>
    public partial class WaitingView : Window
    {
        public WaitingView(string message)
        {
            InitializeComponent();
            Message = message;
        }

        public string Message { get => MessageTextBlock.Text; set => MessageTextBlock.Text = value; }
    }
}
