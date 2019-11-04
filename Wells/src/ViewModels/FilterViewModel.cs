using Wells.BaseView.Validators;
using Wells.BaseView.ViewModel;
using Wells.Base;
using Wells.View.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Wells.BaseView;

namespace Wells.View.ViewModels
{
    public class FilterViewModel : BaseViewModel
    {
        private int _SelectedPropertyIndex;
        private readonly Dictionary<string, PropertyInfo> _Properties;

        public bool ShowStringPanel { get; set; }
        public bool ShowNumericPanel { get; set; }
        public bool ShowBooleanPanel { get; set; }
        public bool ShowDatePanel { get; set; }
        public bool ShowEnumPanel { get; set; }
        public double NumericValue { get; set; }
        public string StringValue { get; set; }
        public int SelectedEnumValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool BooleanValue { get; set; }
        public Type FilterType { get; set; }
        public int SelectedMathFunction { get; set; }
        public string PropertyName { get; set; }
        public string PropertyDisplayName { get; set; }
        public bool IsIntegerNumericType { get; set; }
        public bool IsCreatingFilter { get; set; }
        public string DialogTitle { get; set; }
        public List<string> PropertiesNames
        {
            get
            {
                var list = _Properties.Keys.ToList();
                list.Sort();
                return list;
            }
        }

        public List<string> EnumValues
        {
            get
            {
                if (FilterType.IsEnum)
                {
                    return Common.EnumDescriptionsToList(FilterType);
                }
                return null;
            }
        }

        public List<string> MathFunctionsNames
        {
            get
            {
                return Common.EnumDescriptionsToList(typeof(NumericFunctions));
            }
        }

        public int SelectedPropertyIndex
        {
            get => _SelectedPropertyIndex;
            set
            {
                _SelectedPropertyIndex = value;
                OnPropertySelected();
                NotifyPropertyChanged(nameof(SelectedPropertyIndex));
            }
        }

        public FilterViewModel(Dictionary<string, PropertyInfo> properties) : base(null)
        {
            _Properties = properties;
            EndDate = DateTime.Today;
            BooleanValue = true;
            SelectedEnumValue = 0;
            IsIntegerNumericType = false;
            IsCreatingFilter = true;
            if (properties.Any())
            {
                SelectedPropertyIndex = 0;
            }
            DialogTitle = "Crear filtro";
        }

        public static FilterViewModel CreateInstance<T>(BaseFilter<T> filter, Dictionary<string, PropertyInfo> properties)
        {
            var vm = new FilterViewModel(properties);
            vm.SelectedPropertyIndex = vm.PropertiesNames.IndexOf(filter.DisplayPropertyName);
            vm.IsCreatingFilter = false;
            vm.DialogTitle = "Editar filtro";
            return vm;
        }

        private void OnPropertySelected()
        {
            PropertyDisplayName = PropertiesNames[SelectedPropertyIndex];
            PropertyName = _Properties[PropertyDisplayName].Name;
            FilterType = _Properties[PropertyDisplayName].PropertyType;
            if (Common.IsNumericType(FilterType))
            {
                ShowNumericPanel = true;
                ShowStringPanel = false;
                ShowDatePanel = false;
                ShowBooleanPanel = false;
                ShowEnumPanel = false;
                IsIntegerNumericType = Common.IsIntegerNumericType(FilterType);
            }
            else
            {
                if (FilterType.IsEnum)
                {
                    ShowNumericPanel = false;
                    ShowStringPanel = false;
                    ShowDatePanel = false;
                    ShowBooleanPanel = false;
                    ShowEnumPanel = true;
                    NotifyPropertyChanged(nameof(EnumValues));
                }
                else if (FilterType == typeof(string))
                {
                    ShowNumericPanel = false;
                    ShowStringPanel = true;
                    ShowDatePanel = false;
                    ShowBooleanPanel = false;
                    ShowEnumPanel = false;
                }
                else if (FilterType == typeof(DateTime))
                {
                    ShowNumericPanel = false;
                    ShowStringPanel = false;
                    ShowDatePanel = true;
                    ShowEnumPanel = false;
                    ShowBooleanPanel = false;
                }
                else if (FilterType == typeof(bool))
                {
                    ShowNumericPanel = false;
                    ShowStringPanel = false;
                    ShowDatePanel = false;
                    ShowEnumPanel = false;
                    ShowBooleanPanel = true;
                }
                else
                {
                    View.ShowErrorMessageBox("No se puede filtrar por esta propiedad.");
                }
            }

            NotifyPropertyChanged(nameof(ShowDatePanel));
            NotifyPropertyChanged(nameof(ShowStringPanel));
            NotifyPropertyChanged(nameof(ShowNumericPanel));
            NotifyPropertyChanged(nameof(ShowBooleanPanel));
            NotifyPropertyChanged(nameof(ShowEnumPanel));
        }


        public ICommand SaveFilterCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    CloseModalViewCommand.Execute(true);
                }, (obj) => IsValidFilter(), OnError);
            }
        }

        bool IsValidFilter()
        {
            if (ShowStringPanel)
            {
                return !string.IsNullOrEmpty(StringValue);
            }
            return true;
        }

        protected override void SetCommandUpdates() {
            Add(nameof(StringValue), SaveFilterCommand);
            Add(nameof(ShowStringPanel), SaveFilterCommand);
        }

        protected override void SetValidators() {
           //Nothing to add yet.
        }
    }
}
