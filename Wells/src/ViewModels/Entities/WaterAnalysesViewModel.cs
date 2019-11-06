using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Wells.Persistence.Repositories;
using Wells.BaseView;
using Wells.BaseView.ViewInterfaces;
using Wells.View.Filters;
using Wells.View.Importer;
using Wells.Model;

namespace Wells.View.ViewModels
{
    public class WaterAnalysesViewModel : EntitiesViewModel<WaterAnalysis>
    {
        public WaterAnalysesViewModel() : base(null)
        {
            IsNewCommandEnabled = false;
            IsRemoveCommandEnabled = true;
            FilterCollection = new FilterCollection<WaterAnalysis>();
            Initialize();
            _Entities = Repository.WaterAnalyses.All;
            _ShowWellPanel = true;
            RepositoryWrapper.Instance.Wells.OnEntityRemoved += OnWellRemoved;
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

        public override bool IsNewCommandEnabled { get; }

        public override bool IsRemoveCommandEnabled { get; }

        public override ICommand NewEntityCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {

                }, (obj) => IsNewCommandEnabled, OnError);
            }
        }

        public override ICommand EditEntityCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {

                }, (obj) => SelectedEntity != null, OnError);
            }
        }

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
                    if (SharedBaseView.ShowYesNoMessageBox(MainWindow, "¿Está seguro de eliminar este análisis?", "Eliminar"))
                    {
                        Repository.WaterAnalyses.Remove(SelectedEntity);
                        RepositoryWrapper.Instance.SaveChanges();
                        UpdateEntites();
                    }
                }, (obj) => SelectedEntity != null && IsRemoveCommandEnabled, OnError);
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
                        var vm = new PiperSchoellerGraphicViewModel(new List<WaterAnalysis>() { SelectedEntity });
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
            var piperMenuItem = new MenuItem() { Header = "Piper-Schöeller", Command = OpenPiperShoellerGraphicCommand, CommandParameter = SelectedEntities };
            menu.Items.Add(piperMenuItem);
            menu.Items.Add(new Separator());
            var editWellMenuItem = new MenuItem() { Header = "Editar pozo...", Command = EditWellCommand };
            menu.Items.Add(editWellMenuItem);

            if (IsRemoveCommandEnabled)
            {
                menu.Items.Add(new Separator());
                var removeMenuItem = new MenuItem() { Header = "Eliminar", Command = RemoveEntityCommand };
                menu.Items.Add(removeMenuItem);
                return menu;
            }
            return null;
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
            var wellFilter = new WellFilter<WaterAnalysis>(Repository.WaterAnalyses, false, WellType, WellProperty, SelectedWellName);
            OnCreatingFilter(wellFilter);
        }

        protected override void UpdateEntites()
        {
            _Entities = Repository.WaterAnalyses.All;
            NotifyPropertyChanged(nameof(Entities));
        }
    }
}
