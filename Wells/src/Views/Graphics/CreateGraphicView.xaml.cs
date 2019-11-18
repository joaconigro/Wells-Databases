using System.Windows;
using Wells.BaseView.ViewInterfaces;
using Wells.View.ViewModels;

namespace Wells.View
{
    /// <summary>
    /// Interaction logic for CreatePremadeGraphicView.xaml
    /// </summary>
    public partial class CreateGraphicView : Window, IView
    {
        readonly CreateGraphicViewModel viewModel;
        public CreateGraphicView()
        {
            InitializeComponent();
            viewModel = new CreateGraphicViewModel(this);
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
