using System;
using System.Collections.Generic;
using System.Windows.Input;
using Wells.BaseView;
using Wells.BaseView.ViewInterfaces;
using Wells.BaseView.ViewModel;
using Wells.Model;
using Wells.Persistence;
using Wells.Persistence.Repositories;
using Wells.View.Importer;

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

        public void ExportRejectedToExcel(List<RejectedEntity> rejected)
        {
            if (mainWindow.ShowYesNoMessageBox($"No se pudieron importar {rejected.Count} registro(s). ¿Desea exportar estos datos a un nuevo archivo Excel?", "Datos rechazados"))
            {
                var filename = mainWindow.SaveFileDialog("Archivos de Excel|*.xlsx", "Datos rechazados");
                if (!string.IsNullOrEmpty(filename))
                {
                    ExcelReader.ExportRejectedToExcel(rejected, filename);
                }
            }
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
