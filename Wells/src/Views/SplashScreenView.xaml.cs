using System.Windows;
using Wells.BaseView.ViewInterfaces;
using Wells.View.ViewModels;

namespace Wells.View.Views
{
    /// <summary>
    /// Interaction logic for SplashScreenView.xaml
    /// </summary>
    public partial class SplashScreenView : Window, IView
    {
        readonly SplashScreenViewModel viewModel;
        public SplashScreenView()
        {
            InitializeComponent();
            viewModel = new SplashScreenViewModel(this);
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
    }
}
