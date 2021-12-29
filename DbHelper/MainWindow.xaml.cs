using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wells.BaseView.ViewInterfaces;

namespace Wells.DbHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindow
    {
        private WaitingView? waitingDialog;
        private readonly MainWindowViewModel? viewModel;
        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainWindowViewModel(this);
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

        public void ShowWaitingMessage(string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                IsEnabled = false;
                waitingDialog = new WaitingView(message) { Owner = this };
                waitingDialog.Show();
            });
        }

        public void CloseWaitingMessage()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                waitingDialog?.Close();
                waitingDialog = null;
                IsEnabled = true;
            });
        }
    }

    public interface IMainWindow : IView
    {
        void ShowWaitingMessage(string message);
        void CloseWaitingMessage();
    }
}
