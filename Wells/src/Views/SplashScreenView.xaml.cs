using System.Timers;
using System.Windows;

namespace Wells.View.Views
{
    /// <summary>
    /// Interaction logic for SplashScreenView.xaml
    /// </summary>
    public partial class SplashScreenView : Window
    {
        private Timer timer;
        public SplashScreenView()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, System.EventArgs e)
        {
            timer = new Timer(2000);
            timer.Elapsed += (s, e) =>
            {
                timer.Stop();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Close();
                });
            };
            timer.Start();
        }
    }
}
