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
    public class PrecipitationsViewModel : EntitiesViewModel<Precipitation>
    {
        public PrecipitationsViewModel() : base(null)
        {
            IsNewCommandEnabled = false;
            IsRemoveCommandEnabled = true;
            FilterCollection = new FilterCollection<Precipitation>();
            Initialize();
            _Entities = Repository.Precipitations.All;
            _ShowWellPanel = false;
        }

        protected override void OnSetView(IView view)
        {
            base.OnSetView(view);
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
                    if (MainWindow.ShowYesNoMessageBox("¿Está seguro de eliminar este dato?", "Eliminar"))
                    {
                        Repository.Precipitations.Remove(SelectedEntity);
                        UpdateEntites();
                    }
                }, (obj) => SelectedEntity != null && IsRemoveCommandEnabled, OnError);
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


        protected override void CreateWellFilter() { }

        void UpdateEntites()
        {
            _Entities = Repository.Precipitations.All;
            NotifyPropertyChanged(nameof(Entities));
        }
    }
}
