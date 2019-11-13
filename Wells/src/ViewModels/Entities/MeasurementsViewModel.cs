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
using Microsoft.VisualBasic;

namespace Wells.View.ViewModels
{
    public class MeasurementsViewModel : EntitiesViewModel<Measurement>
    {
        public MeasurementsViewModel() : base(null)
        {
            IsNewCommandEnabled = true;
            IsRemoveCommandEnabled = true;
            //_FilterCollection = new FilterCollection<Measurement>();
            ReadFilters(Information.TypeName(this));
            Initialize();
            _Entities = Repository.Measurements.All;
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
                    var vm = new EditMeasurementViewModel();
                    if (MainWindow.OpenEditEntityDialog(vm))
                    {
                        UpdateEntites();
                    }
                }, (obj) => IsNewCommandEnabled, OnError);
            }
        }

        public override ICommand EditEntityCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    var vm = new EditMeasurementViewModel(SelectedEntity);
                    if (MainWindow.OpenEditEntityDialog(vm))
                    {
                        UpdateEntites();
                    }
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
                    if (SharedBaseView.ShowYesNoMessageBox(MainWindow, "¿Está seguro de eliminar eliminar esta medición?", "Eliminar"))
                    {
                        Repository.Measurements.Remove(SelectedEntity);
                        RepositoryWrapper.Instance.SaveChanges();
                        UpdateEntites();
                    }
                }, (obj) => SelectedEntity != null && IsRemoveCommandEnabled, OnError);
            }
        }

        public override Dictionary<string, PropertyInfo> FilterProperties => Measurement.Properties;

        public override Measurement SelectedEntity { get => _SelectedEntity; set { SetValue(ref _SelectedEntity, value); NotifyPropertyChanged(nameof(WellExistsInfo)); } }

        protected override void SetCommandUpdates()
        {
            base.SetCommandUpdates();
        }

        public override ContextMenu GetContextMenu()
        {
            var menu = new ContextMenu();
            var editMenuItem = new MenuItem { Header = "Editar...", Command = EditEntityCommand };
            var editWellMenuItem = new MenuItem { Header = "Editar pozo...", Command = EditWellCommand };
            var exportMenuItem = new MenuItem { Header = "Exportar...", Command = ExportEntitiesCommand };
            var removeMenuItem = new MenuItem { Header = "Eliminar", Command = RemoveEntityCommand };
            menu.Items.Add(editMenuItem);
            menu.Items.Add(editWellMenuItem);
            menu.Items.Add(new Separator());
            menu.Items.Add(exportMenuItem);
            menu.Items.Add(new Separator());
            menu.Items.Add(removeMenuItem);
            return menu;
        }


        protected override async Task ReadExcelFile(XSSFWorkbook workbook, int sheetIndex)
        {
            ShowWaitingMessage("Leyendo mediciones del archivo Excel...");
            var measurements = await Task.Run(() => ExcelReader.ReadMeasurements(workbook, sheetIndex, null));
            CloseWaitingMessage();

            if (measurements.Any())
            {
                ShowWaitingMessage("Importando mediciones...");
                await Task.Run(() => Repository.Measurements.AddRangeAsync(measurements));
                CloseWaitingMessage();

                ShowWaitingMessage("Guardando base de datos...");
                await Repository.SaveChangesAsync();
            }

            workbook.Close();
            CloseWaitingMessage();
        }


        protected override void CreateWellFilter()
        {
            var wellFilter = new WellFilter<Measurement>(false, WellType, WellProperty, SelectedWellName);
            OnCreatingFilter(wellFilter);
        }

        protected override void UpdateEntites()
        {
            _Entities = Repository.Measurements.All;
            NotifyPropertyChanged(nameof(Entities));
        }        
    }
}
