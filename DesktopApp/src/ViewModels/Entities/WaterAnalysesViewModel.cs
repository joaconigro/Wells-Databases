using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Wells.CoreView;
using Wells.CoreView.ViewInterfaces;
using Wells.View.Filters;
using Wells.View.Importer;
using Wells.YPFModel;

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
        }

        protected override void OnSetView(IView view)
        {
            base.OnSetView(view);
        }

        public override string WellExistsInfo
        {
            get
            {
                if (_SelectedEntity != null)
                    return _SelectedEntity.Well.Exists ? "Pozo existente" : "Pozo inexistente";
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

        public override ICommand ImportEntitiesCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    XSSFWorkbook wb = null;
                    int sheetIndex = -1;

                    if (OpenExcelFile(ref wb, ref sheetIndex))
                    {
                        ReadExcelFile(wb, sheetIndex);
                        UpdateEntites();
                    }
                }, (obj) => IsNewCommandEnabled, OnError, CloseWaitingMessage);
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

        public override ICommand RemoveEntityCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    if (MainWindow.ShowYesNoMessageBox("¿Está seguro de eliminar este análisis?", "Eliminar"))
                    {
                        Repository.WaterAnalyses.Remove(SelectedEntity);
                        UpdateEntites();
                    }
                }, (obj) => SelectedEntity != null && IsRemoveCommandEnabled, OnError);
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
            if (IsRemoveCommandEnabled)
            {
                var menu = new ContextMenu();
                var removeMenuItem = new MenuItem() { Header = "Eliminar", Command = RemoveEntityCommand };
                menu.Items.Add(removeMenuItem);
                return menu;
            }
            return null;
        }


        async void ReadExcelFile(XSSFWorkbook workbook, int sheetIndex)
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

        void UpdateEntites()
        {
            _Entities = Repository.WaterAnalyses.All;
            NotifyPropertyChanged(nameof(Entities));
        }
    }
}
