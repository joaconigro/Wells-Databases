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
using Wells.View.Filters;
using Wells.View.Importer;

namespace Wells.View.ViewModels
{
    public class WaterAnalysesViewModel : EntitiesViewModel<WaterAnalysis>
    {
        public WaterAnalysesViewModel() : base(null)
        {
            ReadFilters(Information.TypeName(this));
            Initialize();
            _Entities = Repository.WaterAnalyses.All;
            _ShowWellPanel = true;
            RepositoryWrapper.Instance.Wells.OnEntityRemoved += OnWellRemoved;
            RepositoryWrapper.Instance.WaterAnalyses.OnEntityRemoved += OnEntitiesRemoved;
        }

        protected override void OnSetView(IView view)
        {
            base.OnSetView(view);
        }

        public override string WellExistsInfo
        {
            get
            {
                if (_SelectedEntity != null) { return _SelectedEntity.Well.Exists ? "Pozo existente" : "Pozo inexistente"; }
                return string.Empty;
            }
        }

        public override bool IsNewCommandEnabled => false;
        public override bool IsEditCommandEnabled => false;

        public ICommand EditWellCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    var vm = new EditWellViewModel(SelectedEntity.Well);
                    if (MainWindow.OpenEditEntityDialog(vm))
                    {
                        UpdateEntites();
                    }
                }, (obj) => SelectedEntity != null, OnError);
            }
        }

        public override ICommand RemoveEntityCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    RemoveEntities(Repository.WaterAnalyses);
                }, (obj) => SelectedEntity != null || (SelectedEntities != null && SelectedEntities.Any()), OnError);
            }
        }

        public ICommand OpenPiperShoellerGraphicCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    if (SelectedEntities != null && SelectedEntities.Any())
                    {
                        var vm = new PiperSchoellerGraphicViewModel(SelectedEntities);
                        MainWindow.OpenGraphicsView(vm);
                    }
                    else if (SelectedEntity != null)
                    {
                        var vm = new PiperSchoellerGraphicViewModel(new List<WaterAnalysis> { SelectedEntity });
                        MainWindow.OpenGraphicsView(vm);
                    }
                }, (obj) => (SelectedEntities != null && SelectedEntities.Any()) || SelectedEntity != null, OnError);
            }
        }

        public override Dictionary<string, PropertyInfo> FilterProperties => WaterAnalysis.Properties;

        public override WaterAnalysis SelectedEntity { get => _SelectedEntity; set { SetValue(ref _SelectedEntity, value); NotifyPropertyChanged(nameof(WellExistsInfo)); } }

        protected override void SetCommandUpdates()
        {
            base.SetCommandUpdates();
        }

        public override ContextMenu GetContextMenu()
        {
            var menu = new ContextMenu();
            var piperMenuItem = new MenuItem { Header = "Piper-Schöeller", Command = OpenPiperShoellerGraphicCommand, CommandParameter = SelectedEntities };
            menu.Items.Add(piperMenuItem);

            menu.Items.Add(new Separator());

            var editWellMenuItem = new MenuItem { Header = "Editar pozo...", Command = EditWellCommand };
            var removeMenuItem = new MenuItem { Header = "Eliminar", Command = RemoveEntityCommand };
            var exportMenuItem = new MenuItem { Header = "Exportar...", Command = ExportEntitiesCommand };

            menu.Items.Add(editWellMenuItem);
            menu.Items.Add(new Separator());
            menu.Items.Add(exportMenuItem);
            menu.Items.Add(new Separator());
            menu.Items.Add(removeMenuItem);
            return menu;
        }


        protected override async Task ReadExcelFile(XSSFWorkbook workbook, int sheetIndex)
        {
            ShowWaitingMessage("Leyendo análisis de agua del archivo Excel...");
            var analyses = await Task.Run(() => ExcelReader.ReadWaterAnalysis(workbook, sheetIndex, null));
            CloseWaitingMessage();

            if (analyses.Any())
            {
                ShowWaitingMessage("Importando análisis...");
                await Task.Run(() => Repository.WaterAnalyses.AddRangeAsync(analyses));
                CloseWaitingMessage();

                ShowWaitingMessage("Guardando base de datos...");
                await Repository.SaveChangesAsync();
            }

            workbook.Close();
            CloseWaitingMessage();
        }


        protected override void CreateWellFilter()
        {
            var wellFilter = new WellFilter<WaterAnalysis>(false, SelectedWellName);
            OnCreatingFilter(wellFilter);
        }

        protected override void UpdateEntites()
        {
            _Entities = Repository.WaterAnalyses.All;
            NotifyPropertyChanged(nameof(Entities));
        }
    }
}
