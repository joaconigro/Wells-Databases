using System.Windows;
using Wells.BaseView;
using Wells.BaseView.ViewInterfaces;
using Wells.View.ViewModels;

namespace Wells.View
{
    /// <summary>
    /// Interaction logic for EditMeasurementView.xaml
    /// </summary>
    public partial class EditMeasurementView : Window, IView
    {
        readonly EditMeasurementViewModel viewModel;
        public EditMeasurementView(EditMeasurementViewModel vm)
        {
            InitializeComponent();

            viewModel = vm;
            viewModel.SetView(this);
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
