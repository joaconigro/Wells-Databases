using DBMigrator;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Wells.BaseView;
using Wells.BaseView.ViewInterfaces;
using Wells.BaseView.ViewModel;
using Wells.Persistence;

namespace Wells.DbMigrator
{
    internal class MainWindowViewModel : BaseViewModel
    {
        private IMainWindow mainWindow;
        private bool removeDbAfterMigrate;
        private Dictionary<string, SqlDbContext> sqlContexts;
        private string selectedDb;

        public MainWindowViewModel(IView view) : base(view)
        {
            Initialize();
        }

        protected override void OnSetView(IView view)
        {
            mainWindow = (IMainWindow)view;
        }

        public bool RemoveDbAfterMigrate
        {
            get => removeDbAfterMigrate;
            set
            {
                SetValue(ref removeDbAfterMigrate, value);
            }
        }

        public List<string> DbNames { get; private set; }

        public string SelectedDb { get => selectedDb; set => SetValue(ref selectedDb, value); }

        protected override void Initialize()
        {
            base.Initialize();
            SetDbContexts();
        }

        protected override void SetCommandUpdates()
        {
            Add(nameof(SelectedDb), MigrateDbCommand);
        }

        void SetDbContexts()
        {
            sqlContexts = new Dictionary<string, SqlDbContext>();
            foreach (var connection in App.Settings.ConnectionStrings)
            {
                var sql = new SqlDbContext(connection.Value);
                var sqlite = new SqliteDbContext(connection.Key);

                if (sql.Database.CanConnect() && !File.Exists(sqlite.DbFilename))
                {
                    sqlContexts.Add(connection.Key, sql);
                }

                sqlite.Dispose();
            }

            DbNames = sqlContexts.Keys.ToList();
        }

        public ICommand MigrateDbCommand
        {
            get => new AsyncCommand(async () =>
            {
                if (RemoveDbAfterMigrate && !SharedBaseView.ShowYesNoMessageBox(mainWindow, "Seleccionó la opción 'Eliminar la base de datos SQL después de migrar', por lo tanto se eliminará la base de datos SQL. Esta operación no se puede deshacer. ¿Desea continuar?", "Advertencia"))
                {
                    return;
                }

                mainWindow.ShowWaitingMessage("Por favor, espere mientras se realiza la migración...");

                var sqliteContext = new SqliteDbContext(SelectedDb);
                if (!File.Exists(sqliteContext.DbFilename))
                {
                    var sqlDbContext = sqlContexts[SelectedDb];
                    sqliteContext.Database.Migrate();

                    sqliteContext.Precipitations.AddRange(sqlDbContext.Precipitations);
                    sqliteContext.Files.AddRange(sqlDbContext.Files);
                    sqliteContext.WaterAnalyses.AddRange(sqlDbContext.WaterAnalyses);
                    sqliteContext.SoilAnalyses.AddRange(sqlDbContext.SoilAnalyses);
                    sqliteContext.FlnaAnalyses.AddRange(sqlDbContext.FlnaAnalyses);
                    sqliteContext.Measurements.AddRange(sqlDbContext.Measurements);
                    sqliteContext.Wells.AddRange(sqlDbContext.Wells);

                    await sqliteContext.SaveChangesAsync();

                    if (RemoveDbAfterMigrate)
                    {
                        await sqlDbContext.Database.EnsureCreatedAsync();
                    }

                    SetDbContexts();
                }
                mainWindow.CloseWaitingMessage();
            }, () => !string.IsNullOrEmpty(SelectedDb), OnError, () => 
            { 
                mainWindow.CloseWaitingMessage();
                NotifyPropertyChanged(nameof(DbNames));
                SelectedDb = string.Empty;
            });
        }
    }
}
