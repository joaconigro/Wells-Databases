using LiveCharts;
using LiveCharts.Definitions.Series;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Wells.BaseView;
using Wells.BaseView.ViewInterfaces;
using Wells.Model;
using Wells.Persistence.Repositories;
using Wells.View.Graphics;

namespace Wells.View.ViewModels
{
    public class ChartGraphicsViewModel : BaseGraphicsViewModel
    {
        private string selectedSeriesDataName;
        private string selectedYParameter;
        private int selectedFromOption;
        private string selectedWellName;
        private Well _Well;
        private string selectedXParameter;

        public string Title { get; }
        public bool IsEditable { get; }
        public List<string> SeriesDataNames => new List<string> { "Mediciones", "Análisis de agua" };
        public List<string> FromOptions => new List<string> { "Pozos", "Precipitaciones" };
        public List<string> WellNames => RepositoryWrapper.Instance.Wells.Names;
        public bool ShowWellOptions => SelectedFromOption == 0 ? true : false;
        public string SelectedYParameter { get => selectedYParameter; set { SetValue(ref selectedYParameter, value); } }
        public string SelectedXParameter { get => selectedXParameter; set { SetValue(ref selectedXParameter, value); } }
        public string SelectedWellName { get => selectedWellName; set { SetValue(ref selectedWellName, value); SetWell(); } }

        public int SelectedFromOption
        {
            get => selectedFromOption;
            set
            {
                SetValue(ref selectedFromOption, value);
                NotifyPropertyChanged(nameof(ShowWellOptions));
            }
        }


        public string SelectedSeriesDataName
        {
            get => selectedSeriesDataName;
            set
            {
                SetValue(ref selectedSeriesDataName, value);
                NotifyPropertyChanged(nameof(YParameters));
                NotifyPropertyChanged(nameof(XParameters));
            }
        }


        public List<string> XParameters
        {
            get
            {
                var values = YParameters.ToList();
                if (AreDateOptionsEnabled)
                {
                    values.Insert(0, "Fecha");
                }
                return values;
            }
        }

        public List<string> YParameters
        {
            get
            {
                return selectedSeriesDataName switch
                {
                    "Mediciones" => Measurement.DoubleProperties.Keys.ToList(),
                    _ => WaterAnalysis.DoubleProperties.Keys.ToList(),
                };
            }
        }

        public Visibility XParametersVisibility
        {
            get
            {
                return (bool)_SeriesInfo?.Any() ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public bool AreDateOptionsEnabled
        {
            get
            {
                bool result = true;
                if ((bool)_SeriesInfo?.Any())
                {
                    result = _SeriesInfo.First().Value.IsDateBased;
                }
                return result;
            }
        }

        void SetWell()
        {
            if (!string.IsNullOrEmpty(selectedWellName))
            {
                _Well = RepositoryWrapper.Instance.Wells.FindByName(selectedWellName);
            }
            else
            {
                _Well = null;
            }
        }

        protected override void SetCommandUpdates()
        {
            Add(nameof(SelectedSerie), RemoveSeriesCommand);
        }

        public ChartGraphicsViewModel(IView view) : base(view)
        {
            Initialize();
            Title = "Gráficos";
            IsEditable = true;
        }

        public ChartGraphicsViewModel(IView view, Well well, SeriesInfoCollection series) : base(view)
        {
            Initialize();
            _Well = well;
            Title = well.Name;
            IsEditable = false;
            selectedXParameter = series.Values.First().DisplayXName;
            foreach (var gs in series.Values)
            {
                ISeriesView genericSeries = CreateSeries(gs.IsFromWell, well, gs.ListName, gs.DisplayXName, gs.DisplayYName);

                if (genericSeries != null)
                {
                    if (genericSeries is LineSeries)
                    {
                        (genericSeries as LineSeries).SetColor(gs.Color);
                    }
                    else
                    {
                        (genericSeries as ColumnSeries).SetColor(gs.Color);
                    }
                }
            }
        }

        void OnSeriesCollectionChanged()
        {
            if (!(bool)_SeriesInfo?.Any())
            {
                var useDateFormat = SelectedFromOption == 1 || selectedXParameter == "Fecha";
                SetFormatters(useDateFormat);
            }
            else
            {
                if (AreDateOptionsEnabled)
                {
                    SelectedXParameter = "Fecha";
                }
            }
        }

        ISeriesView CreateSeries(bool isFromWell, Well well, string listName, string xParam, string yParam)
        {
            OnSeriesCollectionChanged();

            ISeriesView genericSeries;
            if (isFromWell)
            {
                genericSeries = listName switch
                {
                    "Mediciones" => CreateSeriesFromMeasurements(well, xParam, yParam),
                    _ => CreateSeriesFromWaterAnalyses(well, xParam, yParam),
                };
            }
            else
            {
                genericSeries = CreateSeriesFromPrecipitations();
            }

            if (genericSeries != null)
            {
                SeriesCollection.Add(genericSeries);
                NotifyPropertyChanged(nameof(AreDateOptionsEnabled));
                NotifyPropertyChanged(nameof(XParametersVisibility));
            }
            return genericSeries;
        }

        public ICommand CreateSeriesCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    CreateSeries(ShowWellOptions, _Well, SelectedSeriesDataName, SelectedXParameter, SelectedYParameter);
                }, (obj) => true, OnError);
            }
        }

        public ICommand RemoveSeriesCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    OnRemovingSeries(SelectedSerie);
                    OnSeriesCollectionChanged();
                    SeriesCollection.Remove(SelectedSerie);
                    SelectedSerie = null;
                    NotifyPropertyChanged(nameof(AreDateOptionsEnabled));
                    NotifyPropertyChanged(nameof(XParametersVisibility));
                }, (obj) => SelectedSerie != null, OnError);
            }
        }

        public ICommand ChangeColorSeriesCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    if (SelectedSerie is LineSeries)
                    {
                        var currentColor = ((SelectedSerie as LineSeries).Stroke as SolidColorBrush).Color.ToDrawingColor();
                        (SelectedSerie as LineSeries).SetColor(SharedBaseView.ShowColorDialog(currentColor));
                    }
                    else
                    {
                        var currentColor = ((SelectedSerie as ColumnSeries).Fill as SolidColorBrush).Color.ToDrawingColor();
                        (SelectedSerie as ColumnSeries).SetColor(SharedBaseView.ShowColorDialog(currentColor));
                    }
                }, (obj) => SelectedSerie != null, OnError);
            }
        }

        public ICommand SaveChartImageCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    _Dialog.SaveImage(string.Empty);
                }, (obj) => true, OnError);
            }
        }
    }
}
