using System;
using System.Reflection;
using System.Collections.Generic;
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
        public string WindowTitle => $"Well Manager - Base de datos: {App.Settings.CurrentDbName}";
        public List<string> DbNames => App.Settings.DbNames;
        public string SelectedDb
        {
            get => App.Settings.CurrentDbName;
            set
            {
                App.Settings.SetCurrentDb(value);
                ChangeDBCommand.Execute(null);
            }
        }


        public MainWindowViewModel(IView view) : base(view)
        {
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
                    if (SharedBaseView.ShowYesNoMessageBox(View, "¿Está seguro que desea resetear completamente la base de datos?", "¿Resetear base de datos?"))
                    {
                        mainWindow.ShowWaitingMessage("Por favor, espere un momento");
                        await RepositoryWrapper.Instance.ResetSchema(SelectedDb);
                    }
                }, () => true, OnError, () => { mainWindow.CloseWaitingMessage(); });
            }
        }

        public ICommand RemoveDBCommand
        {
            get
            {
                return new AsyncCommand(async () =>
                {
                    if (SharedBaseView.ShowYesNoMessageBox(View, "¿Está seguro que desea eliminar completamente la base de datos?", "¿Eliminar base de datos?"))
                    {
                        mainWindow.ShowWaitingMessage("Por favor, espere un momento");
                        var currentDb = SelectedDb;
                        App.Settings.RemoveDb(currentDb);
                        if (!string.IsNullOrEmpty(App.Settings.CurrentDbName))
                        {
                            mainWindow.RemoveEventHandlers();
                            await RepositoryWrapper.Instance.ChangeSchema(SelectedDb);
                            mainWindow.RaiseRepositoryChanged(RepositoryWrapper.Instance);
                        }
                    }
                }, () => true, OnError, 
                () => { 
                    mainWindow.CloseWaitingMessage();
                    NotifyPropertyChanged(nameof(DbNames));
                    NotifyPropertyChanged(nameof(SelectedDb));
                    NotifyPropertyChanged(nameof(WindowTitle));
                });
            }
        }

        public ICommand ChangeDBCommand
        {
            get
            {
                return new AsyncCommand(async () =>
                {
                    mainWindow.ShowWaitingMessage("Por favor, espere un momento");
                    mainWindow.RemoveEventHandlers();
                    await RepositoryWrapper.Instance.ChangeSchema(SelectedDb);
                    mainWindow.RaiseRepositoryChanged(RepositoryWrapper.Instance);
                    NotifyPropertyChanged(nameof(WindowTitle));
                }, () => true, OnError, () => { mainWindow.CloseWaitingMessage(); });
            }
        }

        public ICommand ExportDBToZipCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    var filename = SharedBaseView.SaveFileDialog("DB backup|*.wzp", "Exportar base de datos", SelectedDb);
                    if (!string.IsNullOrEmpty(filename))
                    {
                        RepositoryWrapper.Instance.SaveDbToZip(filename);
                    }
                }, (obj) => true, OnError);
            }
        }

        public ICommand ImportDBFromZipCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    if (SharedBaseView.ShowYesNoMessageBox(mainWindow, "Para realizar la importación debe cerrar la aplicación y abrir el 'DB Helper'. ¿Desea continuar?", "Cerrar la aplicación"))
                    {
                        var dbHelper = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "DbHelper.exe");
                        if (File.Exists(dbHelper))
                        {
                            Process.Start(dbHelper);
                            App.Current.Shutdown();
                        }
                        else
                        {
                            SharedBaseView.ShowOkOnkyMessageBox(mainWindow, $"No se pudo encontrar el ejecutable {dbHelper}.", "Ejecutable no encontrado");
                        }
                    }
                }, (obj) => true, OnError);
            }
        }

        public ICommand CreateNewCommand
        {
            get
            {
                return new AsyncCommand(async () =>
                {
                    var dbName = SharedBaseView.ShowInputBox(mainWindow, "Ingrese el nombre de la nueva base de datos", "Base de datos");
                    if (!string.IsNullOrEmpty(dbName))
                    {
                        mainWindow.ShowWaitingMessage("Por favor, espere un momento");
                        mainWindow.RemoveEventHandlers();
                        await RepositoryWrapper.Instance.ChangeSchema(dbName);
                        App.Settings.SetCurrentDb(dbName);
                        mainWindow.RaiseRepositoryChanged(RepositoryWrapper.Instance);
                    }
                }, () => true, OnError, 
                () => { 
                    mainWindow.CloseWaitingMessage();
                    NotifyPropertyChanged(nameof(DbNames));
                    NotifyPropertyChanged(nameof(SelectedDb));
                    NotifyPropertyChanged(nameof(WindowTitle));
                });
            }
        }
    }
}
