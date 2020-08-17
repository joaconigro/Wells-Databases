using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using Wells.BaseView;
using Wells.BaseView.ViewInterfaces;
using Wells.BaseView.ViewModel;
using Wells.Model;
using Wells.Persistence.Repositories;
using Wells.Resources;

namespace Wells.View.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        IMainWindow mainWindow;

        public bool IsRepositoryOpened => RepositoryWrapper.IsInstatiated;
        public string WindowTitle => $"Wells - Base de datos: {App.Settings.CurrentDbName}";

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

        public ICommand OpenLogsDirectoryCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    var dir = Path.Combine(AppSettings.SettingsDirectory, "Log");
                    if (Directory.Exists(dir))
                    {
                        Process.Start("explorer.exe", dir);
                    }
                    else
                    {
                        SharedBaseView.ShowOkOnkyMessageBox(View, "La carpeta de registros no se creó todavía.", "Carpeta inexistente");
                    }
                }, (obj) => true, OnError);
            }
        }

        public ICommand RecreateDBCommand
        {
            get
            {
                return new AsyncCommand(async () =>
                {
                    if (SharedBaseView.ShowYesNoMessageBox(View, "¿Está seguro que desea eliminar completamente la base de datos?","¿Eliminar base de datos?"))
                    {
                        mainWindow.ShowWaitingMessage("Por favor, espere un momento");
                        await RepositoryWrapper.Instance.DropSchema(App.Settings.CurrentConnectionString);
                    }
                }, () => true, OnError, () => { mainWindow.CloseWaitingMessage(); });
            }
        }
    }
}
