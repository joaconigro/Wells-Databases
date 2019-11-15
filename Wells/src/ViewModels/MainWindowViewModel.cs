﻿using System.Windows.Input;
using Wells.BaseView;
using Wells.BaseView.ViewInterfaces;
using Wells.BaseView.ViewModel;
using Wells.Model;
using Wells.Persistence.Repositories;

namespace Wells.View.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        IMainWindow mainWindow;

        public bool IsRepositoryOpened => RepositoryWrapper.IsInstatiated;
        public string WindowTitle => $"Well Manager - Base de datos: {App.Settings.CurrentDbName}";

        public MainWindowViewModel(IView view) : base(null)
        {
            SetView(view);
            ChemicalAnalysis.CreateParamtersDictionary();
            Initialize();
        }

        protected override void OnSetView(IView view)
        {
            mainWindow = (IMainWindow)view;
        }

        protected override void SetCommandUpdates()
        {
            Add(nameof(IsRepositoryOpened), OpenGraphicsViewCommand);
        }

        public ICommand OpenGraphicsViewCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    mainWindow.OpenGraphicsView();
                }, (obj) => IsRepositoryOpened, OnError);
            }
        }

        public ICommand OpenCreatePremadeGraphicViewCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    mainWindow.OpenCreatePremadeGraphicView();
                }, (obj) => true, OnError);
            }
        }

        public ICommand ManageColorMapsCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    mainWindow.ShowManageColorMapDialog();
                }, (obj) => true, OnError);
            }
        }
    }
}
