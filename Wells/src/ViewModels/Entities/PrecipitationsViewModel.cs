using Microsoft.VisualBasic;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Wells.BaseView;
using Wells.BaseView.ViewInterfaces;
using Wells.Model;
using Wells.Persistence.Repositories;
using Wells.View.Importer;

namespace Wells.View.ViewModels
{
    public class PrecipitationsViewModel : EntitiesViewModel<Precipitation>
    {
        public PrecipitationsViewModel() : base(null)
        {
            ReadFilters(Information.TypeName(this));
            Initialize();
            _Entities = Repository.Precipitations.All;
            _ShowWellPanel = false;
            RepositoryWrapper.Instance.Precipitations.OnEntityRemoved += OnEntitiesRemoved;
        }

        protected override void OnSetView(IView view)
        {
            base.OnSetView(view);
        }

        public override bool IsNewCommandEnabled => false;
        public override bool IsEditCommandEnabled => false;
        public override ICommand RemoveEntityCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    RemoveEntities(Repository.Precipitations);
                }, (obj) => SelectedEntity != null || (SelectedEntities != null && SelectedEntities.Any()), OnError);
            }
        }

        public override Dictionary<string, PropertyInfo> FilterProperties => Precipitation.Properties;

        public override Precipitation SelectedEntity { get => _SelectedEntity; set { SetValue(ref _SelectedEntity, value); } }

        protected override void SetCommandUpdates()
        {
            base.SetCommandUpdates();
        }

        public override ContextMenu GetContextMenu()
        {
            var menu = new ContextMenu();
            var removeMenuItem = new MenuItem { Header = "Eliminar", Command = RemoveEntityCommand };
            var exportMenuItem = new MenuItem { Header = "Exportar...", Command = ExportEntitiesCommand };

            menu.Items.Add(exportMenuItem);
            menu.Items.Add(new Separator());
            menu.Items.Add(removeMenuItem);
            return menu;
        }


        protected override async Task ReadExcelFile(XSSFWorkbook workbook, int sheetIndex)
        {
            ShowWaitingMessage("Leyendo precipitaciones del archivo Excel...");
            var precipitations = await Task.Run(() => ExcelReader.ReadPrecipitations(workbook, sheetIndex, null));
            CloseWaitingMessage();

            if (precipitations.Any())
            {
                ShowWaitingMessage("Importando precipitaciones...");
                await Task.Run(() => Repository.Precipitations.AddRangeAsync(precipitations));
                CloseWaitingMessage();

                ShowWaitingMessage("Guardando base de datos...");
                await Repository.SaveChangesAsync();
            }

            workbook.Close();
            CloseWaitingMessage();
        }


        protected override void CreateWellFilter()
        {
            //No need to implement.
        }

        protected override void UpdateEntites()
        {
            _Entities = Repository.Precipitations.All;
            NotifyPropertyChanged(nameof(Entities));
        }
    }
}
