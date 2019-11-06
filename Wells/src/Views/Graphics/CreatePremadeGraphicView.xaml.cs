﻿using System.Windows;
using Wells.BaseView;
using Wells.BaseView.ViewInterfaces;
using Wells.View.ViewModels;

namespace Wells.View
{
    /// <summary>
    /// Interaction logic for CreatePremadeGraphicView.xaml
    /// </summary>
    public partial class CreatePremadeGraphicView : Window, IView
    {
        readonly CreatePremadeGraphicViewModel viewModel;
        public CreatePremadeGraphicView()
        {
            InitializeComponent();
            viewModel = new CreatePremadeGraphicViewModel(this);
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
