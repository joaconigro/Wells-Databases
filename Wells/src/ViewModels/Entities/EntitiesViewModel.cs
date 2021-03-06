﻿using Microsoft.VisualBasic;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using Wells.Base;
using Wells.BaseModel.Models;
using Wells.BaseView;
using Wells.BaseView.ViewInterfaces;
using Wells.BaseView.ViewModel;
using Wells.Model;
using Wells.Persistence.Repositories;
using Wells.Resources;
using Wells.View.Filters;
using Wells.View.Importer;
using Wells.View.UserControls;

namespace Wells.View.ViewModels
{
    public abstract class EntitiesViewModel<T> : BaseViewModel, IEntitiesViewModel where T : IBusinessObject
    {
        protected RepositoryWrapper Repository;
        protected IEntitiesControl Control;
        protected IMainWindow MainWindow;
        protected FilterCollection<T> _FilterCollection;
        protected int _EntitiesCount;
        protected BaseFilter<T> _SelectedFilter;
        protected IQueryable<T> _Entities;
        protected T _SelectedEntity;
        protected IEnumerable<T> _SelectedEntities;
        protected bool _ShowWellPanel;

        public int EntitiesCount => _EntitiesCount;

        public abstract bool IsNewCommandEnabled { get; }
        public abstract bool IsEditCommandEnabled { get; }
        public virtual ICommand NewEntityCommand { get; }
        public virtual ICommand EditEntityCommand { get; }
        public abstract ICommand RemoveEntityCommand { get; }
        public ICommand ImportEntitiesCommand
        {
            get
            {
                return new AsyncCommand(async () =>
                {
                    XSSFWorkbook wb = null;
                    if (OpenExcelFile(ref wb, out int sheetIndex))
                    {
                        await ReadExcelFile(wb, sheetIndex).ConfigureAwait(false);
                        UpdateEntites();
                        CloseWaitingMessage();
                        if (ExcelReader.RejectedRows.Count > 1)
                        {
                            ExportRejectedToExcel();
                        }
                    }
                }, () => true, OnError, () => { CloseWaitingMessage(); NotifyPropertyChanged(nameof(WellNames)); });
            }
        }

        public ICommand ExportEntitiesCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    var filename = SharedBaseView.SaveFileDialog("Archivos de Excel|*.xlsx", "Exportar datos");
                    if (!string.IsNullOrEmpty(filename))
                    {
                        ExcelReader.ExportEntities(SelectedEntities, filename);
                    }
                }, (obj) => SelectedEntities != null && SelectedEntities.Any(), OnError);
            }
        }

        public abstract Dictionary<string, PropertyInfo> FilterProperties { get; }
        public abstract T SelectedEntity { get; set; }
        public IEnumerable<T> SelectedEntities { get => _SelectedEntities; set { SetValue(ref _SelectedEntities, value); } }

        public IEnumerable<T> Entities
        {
            get
            {
                if ((bool)FilterCollection?.Any())
                {
                    var list = FilterCollection.Apply(_Entities).ToList();
                    _EntitiesCount = list.Count();
                    NotifyEntityCount();
                    return list.ToList();
                }
                _EntitiesCount = _Entities.Count();
                NotifyEntityCount();
                return _Entities.ToList();
            }
        }

        public FilterCollection<T> FilterCollection
        {
            get => _FilterCollection;
            set
            {
                if (_FilterCollection != null)
                {
                    _FilterCollection.FiltersChanged -= OnFiltersChanged;
                }
                SetValue(ref _FilterCollection, value);
                if (_FilterCollection != null)
                {
                    _FilterCollection.FiltersChanged += OnFiltersChanged;
                }
            }
        }

        public BaseFilter<T> SelectedFilter
        {
            get => _SelectedFilter;
            set
            {
                SetValue(ref _SelectedFilter, value);
            }
        }

        public ICommand RemoveFilterCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    FilterCollection.Remove(SelectedFilter);
                    OnFiltersChanged(null, EventArgs.Empty);
                }, (obj) => SelectedFilter != null, OnError);
            }
        }

        public ICommand EditFilterCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    var vm = FilterViewModel.CreateInstance<T>(SelectedFilter, FilterProperties);
                    SetDataToFilterViewModel(vm);
                    if (Control.ShowFilterDialog(vm))
                    {
                        EditFilter(vm);
                        Control.ForceListBoxItemsRefresh();
                        OnFiltersChanged(null, EventArgs.Empty);
                    }
                }, (obj) => SelectedFilter != null && SelectedFilter.IsEditable, OnError);
            }
        }

        public ICommand AddFilterCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    var vm = new FilterViewModel(FilterProperties);
                    if (Control.ShowFilterDialog(vm))
                    {
                        CreateFilter(vm);
                        OnFiltersChanged(null, EventArgs.Empty);
                    }
                }, (obj) => true, OnError);
            }
        }

        public ICommand AddWellFilterCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    CreateWellFilter();
                    OnFiltersChanged(null, EventArgs.Empty);
                }, (obj) =>
                {
                    if (WellType > 0 || WellProperty > 0)
                    {
                        if (WellProperty != 1) { return true; }
                        if (WellProperty == 1 && !string.IsNullOrEmpty(SelectedWellName)) { return true; }
                        return false;
                    }
                    return false;
                }, OnError);
            }
        }

        private void CreateFilter(FilterViewModel vm)
        {
            var f = FilterFactory.CreateFilter<T>(vm);
            OnCreatingFilter(f);
        }

        protected void OnCreatingFilter(IBaseFilter<T> filter)
        {
            if (filter != null)
            {
                FilterCollection.Add(filter);
                filter.ParentCollection = FilterCollection;
            }
        }

        private void EditFilter(FilterViewModel vm)
        {
            SelectedFilter.SetUpdatedValues(vm);
        }

        private void SetDataToFilterViewModel(FilterViewModel vm)
        {
            if (vm.ShowNumericPanel)
            {
                vm.NumericValue = (SelectedFilter as NumericFilter<T>).Value;
                vm.SelectedMathFunction = (int)((SelectedFilter as NumericFilter<T>).Function);
            }
            else if (vm.ShowStringPanel)
            {
                vm.StringValue = (SelectedFilter as StringFilter<T>).Value;
            }
            else if (vm.ShowDatePanel)
            {
                vm.StartDate = (SelectedFilter as DateRangeFilter<T>).StartDate;
                vm.EndDate = (SelectedFilter as DateRangeFilter<T>).EndDate;
            }
            else if (vm.ShowBooleanPanel)
            {
                vm.BooleanValue = (SelectedFilter as BooleanFilter<T>).Value;
            }
            else if (vm.ShowEnumPanel)
            {
                vm.SelectedEnumValue = (SelectedFilter as EnumFilter<T>).Value;
            }
        }

        protected EntitiesViewModel(IView view) : base(view)
        {
            Repository = RepositoryWrapper.Instance;
        }

        protected void NotifyEntityCount()
        {
            NotifyPropertyChanged(nameof(EntitiesCount));
        }

        protected override void OnSetView(IView view)
        {
            Control = (IEntitiesControl)view;
            MainWindow = Control.MainWindow;
            View = MainWindow;
        }

        protected override void SetValidators()
        {
            //No need to implement yet.
        }

        protected override void SetCommandUpdates()
        {
            Add(nameof(SelectedEntity), new List<ICommand> { EditEntityCommand, RemoveEntityCommand });
            Add(nameof(SelectedEntities), RemoveEntityCommand);
            Add(nameof(SelectedFilter), new List<ICommand> { EditFilterCommand, RemoveFilterCommand });
            Add(nameof(WellType), AddWellFilterCommand);
            Add(nameof(WellProperty), AddWellFilterCommand);
            Add(nameof(SelectedWellName), AddWellFilterCommand);
        }

        protected void OnFiltersChanged(object sender, EventArgs e)
        {
            NotifyPropertyChanged(nameof(Entities));
            NotifyPropertyChanged(nameof(FilterCollection));
            SaveFilters(Information.TypeName(this));
        }

        public abstract ContextMenu GetContextMenu();

        public void SetSelectedItems(IEnumerable<object> entities)
        {
            var selection = from object o in entities
                            select (T)o;
            SelectedEntities = selection;
        }

        protected void ShowWaitingMessage(string message)
        {
            Control.MainWindow.ShowWaitingMessage(message);
        }

        protected void CloseWaitingMessage()
        {
            Control.MainWindow.CloseWaitingMessage();
        }

        protected void OnEntitiesRemoved(object sender, EntityRemovedEventArgs<T> eventArgs)
        {
            RepositoryWrapper.Instance.SaveChanges();
            UpdateEntites();
        }

        void ExportRejectedToExcel()
        {
            if (SharedBaseView.ShowYesNoMessageBox(MainWindow, $"No se pudieron importar {ExcelReader.RejectedRows.Count - 1} registro(s). " +
                $"¿Desea exportar estos datos a un nuevo archivo Excel?", "Datos rechazados"))
            {
                var filename = SharedBaseView.SaveFileDialog("Archivos de Excel|*.xlsx", "Datos rechazados");
                if (!string.IsNullOrEmpty(filename))
                {
                    ExcelReader.ExportRejected(filename);
                }
            }
        }

        protected void RemoveEntities(IRepositoryBase<T> repository)
        {
            if (SharedBaseView.ShowYesNoMessageBox(MainWindow, "¿Está seguro de eliminar esto(s) dato(s)?", "Eliminar"))
            {
                if (SelectedEntities != null && SelectedEntities.Any())
                {
                    repository.RemoveRange(SelectedEntities);
                }
                else if (SelectedEntity != null)
                {
                    repository.Remove(SelectedEntity);
                }
            }
        }

        protected abstract void CreateWellFilter();

        protected abstract Task ReadExcelFile(XSSFWorkbook workbook, int sheetIndex);

        protected abstract void UpdateEntites();
        public bool ShowWellPanel => _ShowWellPanel;

        public List<string> WellNames => Repository.Wells.Names;


        public List<string> WellTypes => Common.EnumDescriptionsToList(typeof(WellTypes));


        public List<string> WellProperties => Common.EnumDescriptionsToList(typeof(WellQueryProperty));


        public int WellType { get => wellType; set { SetValue(ref wellType, value); } }


        public int WellProperty { get => wellProperty; set { SetValue(ref wellProperty, value); NotifyPropertyChanged(nameof(WellNamesVisible)); } }


        public string SelectedWellName { get => selectedWellName; set { SetValue(ref selectedWellName, value); } }

        private int wellType;
        private int wellProperty;
        private string selectedWellName;

        public bool WellNamesVisible => wellProperty == (int)WellQueryProperty.Name;


        public virtual string WellExistsInfo => string.Empty;

        protected void OnWellRemoved(object sender, EntityRemovedEventArgs<Well> eventArgs)
        {
            UpdateEntites();
        }

        protected bool OpenExcelFile(ref XSSFWorkbook workbook, out int sheetIndex)
        {
            var filename = SharedBaseView.OpenFileDialog("Archivos de Excel|*.xlsx", "Importar Excel");
            if (!string.IsNullOrEmpty(filename))
            {
                workbook = new XSSFWorkbook(filename);
                if (workbook.NumberOfSheets > 1)
                {
                    List<string> sheets = new List<string>();
                    for (int i = 0; i < workbook.NumberOfSheets; i++)
                    {
                        sheets.Add(workbook.GetSheetName(i));
                    }
                    sheetIndex = MainWindow.SelectSheetDialog(sheets);
                }
                else
                {
                    sheetIndex = 0;
                }
                return sheetIndex > -1;
            }
            sheetIndex = -1;
            return false;
        }


        public void ReadFilters(string filtersFilename)
        {
            var filename = Path.Combine(AppSettings.SettingsDirectory, $"{filtersFilename}.wft");
            FilterCollection = new FilterCollection<T>();

            try
            {
                if (File.Exists(filename))
                {
                    var settings = new XmlReaderSettings { IgnoreWhitespace = true };
                    using var stream = new StreamReader(filename);
                    using var reader = XmlReader.Create(stream, settings);
                    reader.MoveToContent();
                    FilterCollection.ReadXml(reader);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex, false);
            }
        }

        public void SaveFilters(string filtersFilename)
        {
            var filename = Path.Combine(AppSettings.SettingsDirectory, $"{filtersFilename}.wft");

            var xml = new StringBuilder();
            var settings = new XmlWriterSettings { Indent = true };
            try
            {
                using (var writer = XmlWriter.Create(xml, settings))
                {
                    writer.WriteStartElement("FilterCollection");
                    FilterCollection.WriteXml(writer);
                    writer.WriteEndElement();
                }
                File.WriteAllText(filename, xml.ToString());
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex, false);
            }
        }

    }
}
