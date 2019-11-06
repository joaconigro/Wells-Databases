using System;
using System.Windows;
using Wells.BaseView;
using Wells.BaseView.ViewInterfaces;
using Wells.View.ViewModels;

namespace Wells.View
{
    /// <summary>
    /// Interaction logic for FilterEditingView.xaml
    /// </summary>
    public partial class FilterEditingView : Window, IView
    {
        readonly FilterViewModel _ViewModel;

        public FilterEditingView(FilterViewModel viewModel)
        {
            InitializeComponent();

            _ViewModel = viewModel;
            DataContext = _ViewModel;
            _ViewModel.SetView(this);
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

        private void AfterContentRendered(object sender, EventArgs e)
        {
            MathOptionsComboBox.SelectedIndex = _ViewModel.SelectedMathFunction;
        }
    }
}
