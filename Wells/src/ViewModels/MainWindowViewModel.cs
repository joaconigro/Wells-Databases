using System;
using System.Windows.Input;
using Wells.BaseView;
using Wells.BaseView.ViewInterfaces;
using Wells.BaseView.ViewModel;
using Wells.Model;
using Wells.Persistence.Repositories;

namespace Wells.View.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        RepositoryWrapper repository;
        IMainWindow mainWindow;

        public bool RepositoryIsOpened { get; private set; }

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

        public void InitializeContext(string connectionString)
        {
            try
            {
                mainWindow.ShowWaitingMessage("Abriendo la base de datos");
                RepositoryWrapper.Instantiate(connectionString);
                repository = RepositoryWrapper.Instance;
                RepositoryIsOpened = repository != null;
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
            finally
            {
                NotifyPropertyChanged(nameof(RepositoryIsOpened));
            }
        }

        protected override void SetCommandUpdates()
        {
            Add(nameof(RepositoryIsOpened), OpenGraphicsViewCommand);
        }

        protected override void SetValidators()
        {
            //No need to add validators yet.
        }

        public ICommand OpenGraphicsViewCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    mainWindow.OpenGraphicsView();
                }, (obj) => RepositoryIsOpened, OnError);
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
    }
}
