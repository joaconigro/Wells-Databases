using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Definitions.Series;
using LiveCharts.Wpf;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Wells.BaseModel.Models;
using Wells.BaseView.ViewInterfaces;
using Wells.BaseView.ViewModel;
using Wells.Model;
using Wells.Persistence.Repositories;
using Wells.View.Graphics;

namespace Wells.View.ViewModels
{
    public abstract class BaseGraphicsViewModel : BaseViewModel
    {
        protected IChartGraphicsView _Dialog;
        private DateTime _MinimunDate;
        private readonly Random _RandomGenerator = new Random();
        private ISeriesView _SelectedSerie;
        private DateTime _MaximunDate;
        protected Dictionary<ISeriesView, SeriesInfo> _SeriesInfo = new Dictionary<ISeriesView, SeriesInfo>();

        public DateTime MinimunDate
        {
            get => _MinimunDate;
            set
            {
                if (value < _MaximunDate)
                {
                    SetValue(ref _MinimunDate, value);
                    UpdateSeries();
                }
            }
        }


        public DateTime MaximunDate
        {
            get => _MaximunDate;
            set
            {
                if (value > _MinimunDate)
                {
                    SetValue(ref _MaximunDate, value);
                    UpdateSeries();
                }
            }
        }

        public ISeriesView SelectedSerie { get => _SelectedSerie; set { SetValue(ref _SelectedSerie, value); RaiseCommandUpdates(nameof(SelectedSerie)); } }



        public Func<double, string> XFormatter { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public SeriesCollection SeriesCollection { get; set; }

        protected BaseGraphicsViewModel(IView view) : base(view)
        {
            _MaximunDate = DateTime.Today;
            _MinimunDate = DateTime.Today.Subtract(TimeSpan.FromDays(180));

            var dateConfig = Mappers.Xy<DateModel>();
            dateConfig.X(dm => dm.SampleDate.ToOADate());
            dateConfig.Y(dm => dm.Value);

            XFormatter = new Func<double, string>(d => DateTime.FromOADate(d).Date.ToString("dd/MM/yy"));
            YFormatter = new Func<double, string>(d => d.ToString("N2"));

            SeriesCollection = new SeriesCollection(dateConfig);
            _Dialog = (IChartGraphicsView)view;
        }

        void SetAxis(ISeriesView aSeries, string units)
        {
            var axis = _Dialog.GetYAxisIndex(units);
            if (axis != -1)
            {
                aSeries.ScalesYAt = axis;
            }
            else
            {
                var yAxis = new Axis { Title = units, Position = AxisPosition.RightTop, LabelFormatter = YFormatter, FontSize = 12 };
                _Dialog.AddAxis(yAxis);
                axis = _Dialog.GetYAxisIndex(units);
                aSeries.ScalesYAt = axis;
            }
        }

        protected void OnRemovingSeries(ISeriesView aSeries)
        {
            _SeriesInfo.Remove(aSeries);
            var removeAxis = !_SeriesInfo.Keys.Where(s => s.ScalesYAt == aSeries.ScalesYAt).Any();
            if (removeAxis) { _Dialog.RemoveAxis(aSeries.ScalesYAt); }
        }

        List<DateModel> GetValues<T>(IEnumerable<T> list, string propertyName) where T : IBusinessObject
        {
            var values = (from o in list
                          let param = Convert.ToDouble(Interaction.CallByName(o, propertyName, CallType.Get))
                          let date = (DateTime)Interaction.CallByName(o, "Date", CallType.Get)
                          where date >= MinimunDate && date <= MaximunDate && param != BusinessObject.NumericNullValue
                          orderby date ascending
                          select new DateModel(date, param)).ToList();

            return values;
        }

        List<DateModel> GetValuesFromListName(Well well, string listName, string propertyName, string parameter)
        {
            var values = listName switch
            {
                "Mediciones" => GetValues(well.Measurements, propertyName),
                "Análisis de FLNA" => GetValues(well.FlnaAnalyses, propertyName),
                "Análisis de agua" => GetValues(well.WaterAnalyses, propertyName),
                "Análisis de suelos" => GetValues(well.SoilAnalyses, propertyName),
                _ => GetValues(RepositoryWrapper.Instance.Precipitations.All, nameof(Precipitation.Millimeters)),
            };

            return FilterAndValidateValues(values, well, parameter);
        }

        List<DateModel> FilterAndValidateValues(List<DateModel> values, Well well, string parameter)
        {
            if (well != null)
            {
                if (values.Count < 2)
                {
                    throw new Exception($"Hay menos de dos datos de {parameter} para representar en el pozo {well.Name}, por lo tanto no se puede dibujar la línea.");
                }
            }
            else
            {
                var newValues = values.Where(v => v.Value > 0.0).ToList();

                if (newValues.Count < 1)
                {
                    throw new Exception("No hay datos de precipitaciones para representar, por lo tanto no se dibujará la serie.");
                }
                else if (newValues.Count > 200)
                {
                    var filtered = new List<DateModel>();
                    var aStep = Math.Max(1, newValues.Count / 150);
                    for (int i = 0; i < newValues.Count; i += aStep)
                    {
                        filtered.Add(newValues[i]);
                    }
                    return filtered;
                }
            }
            return values;
        }

        protected ISeriesView CreateSeriesFromMeasurements(Well well, string parameter)
        {
            var series = CreateLineSeries();
            series.Title = $"{well.Name} - {parameter}";

            var propertyName = Measurement.Properties[parameter].Name;
            var values = FilterAndValidateValues(GetValues(well.Measurements, propertyName), well, parameter);

            SetAxis(series, "metros");

            series.Values.AddRange(values);
            _SeriesInfo.Add(series, new SeriesInfo(well, "Mediciones", propertyName, parameter, GetValuesFromListName));
            return series;
        }

        protected ISeriesView CreateSeriesFromSoilAnalyses(Well well, string parameter)
        {
            var series = CreateLineSeries();
            series.Title = $"{well.Name} - {parameter}";

            var propertyName = SoilAnalysis.Properties[parameter].Name;
            var values = FilterAndValidateValues(GetValues(well.SoilAnalyses, propertyName), well, parameter);

            var units = SoilAnalysis.GetChemicalAnalysisUnits(propertyName);
            if (!string.IsNullOrEmpty(units)) { SetAxis(series, units); }


            series.Values.AddRange(values);
            _SeriesInfo.Add(series, new SeriesInfo(well, "Análisis de suelos", propertyName, parameter, GetValuesFromListName));
            return series;
        }

        protected ISeriesView CreateSeriesFromWaterAnalyses(Well well, string parameter)
        {
            var series = CreateLineSeries();
            series.Title = $"{well.Name} - {parameter}";


            var propertyName = WaterAnalysis.Properties[parameter].Name;
            var values = FilterAndValidateValues(GetValues(well.WaterAnalyses, propertyName), well, parameter);

            var units = WaterAnalysis.GetChemicalAnalysisUnits(propertyName);
            if (!string.IsNullOrEmpty(units)) { SetAxis(series, units); }


            series.Values.AddRange(values);
            _SeriesInfo.Add(series, new SeriesInfo(well, "Análisis de agua", propertyName, parameter, GetValuesFromListName));
            return series;
        }

        protected ISeriesView CreateSeriesFromFlnaAnalyses(Well well, string parameter)
        {
            var series = CreateLineSeries();
            series.Title = $"{well.Name} - {parameter}";

            var propertyName = FlnaAnalysis.Properties[parameter].Name;
            var values = FilterAndValidateValues(GetValues(well.FlnaAnalyses, propertyName), well, parameter);

            var units = FlnaAnalysis.GetChemicalAnalysisUnits(propertyName);
            if (!string.IsNullOrEmpty(units)) { SetAxis(series, units); }

            series.Values.AddRange(values);
            _SeriesInfo.Add(series, new SeriesInfo(well, "Análisis de FLNA", propertyName, parameter, GetValuesFromListName));
            return series;
        }

        protected ISeriesView CreateSeriesFromPrecipitations()
        {
            var series = CreateColumnSeries();
            series.Title = "Precipitaciones";

            var values = FilterAndValidateValues(GetValues(RepositoryWrapper.Instance.Precipitations.All, nameof(Precipitation.Millimeters)), null, string.Empty);
            SetAxis(series, "mm");

            series.Values.AddRange(values);
            _SeriesInfo.Add(series, new SeriesInfo(GetValuesFromListName));
            return series;
        }


        LineSeries CreateLineSeries()
        {
            var seriesColor = Color.FromArgb(255, (byte)_RandomGenerator.Next(0, 255), (byte)_RandomGenerator.Next(0, 255), (byte)_RandomGenerator.Next(0, 255));

            var lineSeries = new LineSeries
            {
                LineSmoothness = 0,
                Stroke = new SolidColorBrush(seriesColor),
                PointGeometrySize = 8,
                Fill = new SolidColorBrush(Colors.Transparent),
                Values = new ChartValues<DateModel>(),
                StrokeDashArray = new DoubleCollection { 2.0 }
            };

            return lineSeries;
        }

        ColumnSeries CreateColumnSeries()
        {
            var seriesColor = Color.FromArgb(255, (byte)_RandomGenerator.Next(0, 255), (byte)_RandomGenerator.Next(0, 255), (byte)_RandomGenerator.Next(0, 255));

            var columnSeries = new ColumnSeries
            {
                PointGeometry = new RectangleGeometry(new Rect(new Size(8, 8))),
                ColumnPadding = 0,
                Fill = new SolidColorBrush(seriesColor),
                Values = new ChartValues<DateModel>()
            };
            return columnSeries;
        }

        protected void UpdateSeries()
        {
            foreach (var s in SeriesCollection)
            {
                s.Values.Clear();
                s.Values.AddRange(_SeriesInfo[s].GetValues());
            }
            _Dialog?.ResetZoom();
        }



        protected struct SeriesInfo
        {
            public Well Well { get; }
            public string PropertyName { get; }
            public string ParameterName { get; }
            public string ListName { get; }
            public Func<Well, string, string, string, List<DateModel>> GetValuesFunc { get; }

            public SeriesInfo(Well well, string listName, string propertyName, string parameterName, Func<Well, string, string, string, List<DateModel>> func)
            {
                Well = well;
                ListName = listName;
                PropertyName = propertyName;
                ParameterName = parameterName;
                GetValuesFunc = func;
            }

            public SeriesInfo(Func<Well, string, string, string, List<DateModel>> func)
            {
                Well = null;
                ListName = "Precipitaciones";
                PropertyName = string.Empty;
                ParameterName = string.Empty;
                GetValuesFunc = func;
            }

            public List<DateModel> GetValues()
            {
                return GetValuesFunc.Invoke(Well, ListName, PropertyName, ParameterName);
            }
        }
    }


}
