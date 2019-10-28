using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Serialization;
using Wells.CoreView;
using Wells.CoreView.ViewInterfaces;
using Wells.View.Filters;
using Wells.View.Graphics;
using Wells.View.Importer;
using Wells.YPFModel;
using Wells.CorePersistence.Repositories;

namespace Wells.View.ViewModels
{
    public class WellsViewModel : EntitiesViewModel<Well>
    {
        private List<PremadeSeriesInfoCollection> _PremadeGraphics;

        public WellsViewModel() : base(null)
        {
            IsNewCommandEnabled = true;
            IsRemoveCommandEnabled = true;
            FilterCollection = new Filters.FilterCollection<Well>();
            Initialize();
            _Entities = Repository.Wells.All;
            _ShowWellPanel = true;
            _PremadeGraphics = CreatePremadeGraphicViewModel.ReadPremadeGraphics();
        }

        protected override void OnSetView(IView view)
        {
            base.OnSetView(view);
            MainWindow.PremadeGraphicsChanged += OnPremadeGraphicsChanged;
        } 

        public override string WellExistsInfo
        {
            get
            {
                if (_SelectedEntity != null)
                    return _SelectedEntity.Exists ? "Pozo existente" : "Pozo inexistente";
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
                    var vm = new EditWellViewModel();
                    if (MainWindow.OpenEditEntityDialog(vm))
                        UpdateEntites();
                }, (obj) => IsNewCommandEnabled, OnError);
            }
        }

        public override ICommand EditEntityCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    var vm = new EditWellViewModel(SelectedEntity);
                    if (MainWindow.OpenEditEntityDialog(vm))
                        UpdateEntites();

                }, (obj) => SelectedEntity != null, OnError);
            }
        }

        public override ICommand RemoveEntityCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    if (MainWindow.ShowYesNoMessageBox("¿Está seguro de eliminar este pozo?", "Eliminar"))
                    {
                        Repository.Wells.Remove(SelectedEntity);
                        RepositoryWrapper.Instance.SaveChanges();
                        UpdateEntites();
                    }
                }, (obj) => SelectedEntity != null && IsRemoveCommandEnabled, OnError);
            }
        }

        public ICommand OpenPremadeGraphicCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    MainWindow.OpenGraphicsView(SelectedEntity, param as PremadeSeriesInfoCollection);
                }, (obj) => SelectedEntity != null, OnError);
            }
        }

        public ICommand OpenPiperShoellerGraphicCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                if (SelectedEntities != null && SelectedEntities.Any()) {
                    var vm = new PiperSchoellerGraphicViewModel(SelectedEntities);
                    MainWindow.OpenGraphicsView(vm); }
                else if (SelectedEntity != null) {
                    var vm = new PiperSchoellerGraphicViewModel(new List<Well>() { SelectedEntity });
                MainWindow.OpenGraphicsView(vm);
                                                                                         }
                }, (obj) => (SelectedEntities != null && SelectedEntities.Any()) || SelectedEntity != null, OnError);
            }
        }

        public override Dictionary<string, PropertyInfo> FilterProperties => Well.Properties;

        public override Well SelectedEntity { get => _SelectedEntity; set { SetValue(ref _SelectedEntity, value); NotifyPropertyChanged(nameof(WellExistsInfo)); } }

        protected override void SetCommandUpdates()
        {
            base.SetCommandUpdates();
            Add(nameof(SelectedEntity), OpenPremadeGraphicCommand );
            Add(nameof(SelectedEntity), OpenPiperShoellerGraphicCommand);
            Add(nameof(SelectedEntities), OpenPiperShoellerGraphicCommand);
        }

        public override ContextMenu GetContextMenu()
        {
            var menu = new ContextMenu();
            var editMenuItem = new MenuItem() { Header = "Editar...", Command = EditEntityCommand };
            var removeMenuItem = new MenuItem() { Header = "Eliminar", Command = RemoveEntityCommand };
            menu.Items.Add(editMenuItem);
            menu.Items.Add(new Separator());

            var graphicsMenuItem = new MenuItem() { Header = "Gráficos" };
            var piperMenuItem = new MenuItem() { Header = "Piper-Schöeller", Command = OpenPiperShoellerGraphicCommand, CommandParameter = SelectedEntities };
            graphicsMenuItem.Items.Add(piperMenuItem);
            foreach (var pg in _PremadeGraphics)
            {
                graphicsMenuItem.Items.Add(new Separator());
                var aMenuItem = new MenuItem() { Header = pg.Title, Command = OpenPremadeGraphicCommand, CommandParameter = pg };
                graphicsMenuItem.Items.Add(aMenuItem);
            }
            menu.Items.Add(graphicsMenuItem);

            menu.Items.Add(new Separator());
            menu.Items.Add(removeMenuItem);
            return menu;
        }

        protected override async Task ReadExcelFile(XSSFWorkbook workbook, int sheetIndex)
        {
            ShowWaitingMessage("Leyendo pozos del archivo Excel...");
            var wells = await Task.Run(() => ExcelReader.ReadWells(workbook, sheetIndex, null));
            CloseWaitingMessage();

            if (wells.Any())
            {
                ShowWaitingMessage("Importando pozos...");
                await Task.Run(() => Repository.Wells.AddRangeAsync(wells));
                CloseWaitingMessage();

                ShowWaitingMessage("Guardando base de datos...");
                await Repository.SaveChangesAsync();
            }

            workbook.Close();
            CloseWaitingMessage();
        }


        void OnPremadeGraphicsChanged(object sender, EventArgs args)
        {
            _PremadeGraphics = CreatePremadeGraphicViewModel.ReadPremadeGraphics();
            Control.UpdateRowContextMenu();
        }

        protected override void CreateWellFilter()
        {
            var wellFilter = new WellFilter<Well>(Repository.Wells, true, WellType, WellProperty, SelectedWellName);
            OnCreatingFilter(wellFilter);
        }

        protected override void UpdateEntites()
        {
            _Entities = Repository.Wells.All;
            NotifyPropertyChanged(nameof(Entities));
        }
    }
}
