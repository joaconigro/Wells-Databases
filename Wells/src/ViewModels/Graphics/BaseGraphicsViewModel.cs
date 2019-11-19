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
using Wells.BaseView;
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
        private Func<double, string> xFormatter;
        private Func<double, string> yFormatter;
        private SeriesCollection seriesCollection;

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



        public Func<double, string> XFormatter { get => xFormatter; set { SetValue(ref xFormatter, value); } }
        public Func<double, string> YFormatter { get => yFormatter; set { SetValue(ref yFormatter, value); } }
        public SeriesCollection SeriesCollection { get => seriesCollection; set { SetValue(ref seriesCollection, value); } }

        protected BaseGraphicsViewModel(IView view) : base(view)
        {
            _MaximunDate = DateTime.Today;
            _MinimunDate = new DateTime(2000, 1, 1);

            SeriesCollection = new SeriesCollection();
            SetFormatters(true);
             _Dialog = (IChartGraphicsView)view;
        }

        protected void SetFormatters(bool useDateFormatter)
        {
            var dateConfig = Mappers.Xy<DateModel>();

            if (useDateFormatter)
            {
                dateConfig.X(dm => dm.SampleDate.ToOADate());
                XFormatter = new Func<double, string>(d => DateTime.FromOADate(d).Date.ToString("dd/MM/yy"));
            }
            else
            {
                dateConfig.X(dm => dm.X);
                XFormatter = new Func<double, string>(d => d.ToString("N2"));
            }
            dateConfig.Y(dm => dm.Y);
            YFormatter = new Func<double, string>(d => d.ToString("N2"));
            SeriesCollection.Configuration = dateConfig;
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
            if (removeAxis) 
            {
                _Dialog.RemoveAxis(aSeries.ScalesYAt); 
                foreach(var s in _SeriesInfo.Keys)
                {
                    if(s.ScalesYAt > aSeries.ScalesYAt)
                    {
                        s.ScalesYAt -= 1;
                    }
                }
            }
        }

        List<DateModel> GetValues<T>(IEnumerable<T> list, string propertyXName, string propertyYName) where T : IBusinessObject
        {
            var values = from o in list
                          let x = propertyXName != "Date" ? Convert.ToDouble(Interaction.CallByName(o, propertyXName, CallType.Get)) : 0.0
                          let y = Convert.ToDouble(Interaction.CallByName(o, propertyYName, CallType.Get))
                          let date = (DateTime)Interaction.CallByName(o, "Date", CallType.Get)
                          where date >= MinimunDate && date <= MaximunDate && !x.Equals(BusinessObject.NumericNullValue) && !y.Equals(BusinessObject.NumericNullValue)
                          select new DateModel(date, x, y);

            if(propertyXName != "Date")
            {
                values = values.OrderBy(d => d.X);
            }
            else
            {
                values = values.OrderBy(d => d.SampleDate);
            }

            return values.ToList();
        }

        List<DateModel> GetValuesFromListName(Well well, string listName, string propertyXName, string propertyYName, string parameter)
        {
            var values = listName switch
            {
                "Mediciones" => GetValues(well.Measurements, propertyXName, propertyYName),
                "Análisis de FLNA" => GetValues(well.FlnaAnalyses, propertyXName, propertyYName),
                "Análisis de agua" => GetValues(well.WaterAnalyses, propertyXName, propertyYName),
                "Análisis de suelos" => GetValues(well.SoilAnalyses, propertyXName, propertyYName),
                _ => GetValues(RepositoryWrapper.Instance.Precipitations.All, propertyXName, propertyYName),
            };

            return FilterAndValidateValues(values, well, parameter);
        }

        List<DateModel> FilterAndValidateValues(List<DateModel> values, Well well, string parameter)
        {
            if (well != null)
            {
                if (values.Count < 2)
                {
                    SharedBaseView.ShowErrorMessageBox(View, $"Hay menos de dos datos de {parameter} para representar en el pozo {well.Name}, " +
                        $"por lo tanto no se puede dibujar la línea.");
                    return null;
                }
            }
            else
            {
                var newValues = values.Where(v => v.Y > 0.0).ToList();

                if (newValues.Count < 1)
                {
                    SharedBaseView.ShowErrorMessageBox(View, "No hay datos de precipitaciones para representar, por lo tanto no se dibujará la serie.");
                    return null;
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

        protected ISeriesView CreateSeriesFromMeasurements(Well well, string displayNameX, string displayNameY)
        {
            var propertyXName = Measurement.Properties[displayNameX].Name;
            var propertyYName = Measurement.Properties[displayNameY].Name;
            var values = FilterAndValidateValues(GetValues(well.Measurements, propertyXName, propertyYName), well, displayNameY);
            
            if(values != null)
            {
                var series = CreateLineSeries();
                SetAxis(series, "metros");

                series.Values.AddRange(values);
                _SeriesInfo.Add(series, new SeriesInfo(well, "Mediciones", propertyXName, displayNameX, propertyYName, displayNameY, GetValuesFromListName));
                series.Title = _SeriesInfo[series].ToString();
                return series;
            }
            return null;            
        }

        protected ISeriesView CreateSeriesFromSoilAnalyses(Well well, string displayNameX, string displayNameY)
        {
            var propertyXName = SoilAnalysis.Properties[displayNameX].Name;
            var propertyYName = SoilAnalysis.Properties[displayNameY].Name;
            var values = FilterAndValidateValues(GetValues(well.SoilAnalyses, propertyXName, propertyYName), well, displayNameY);

            if (values != null)
            {
                var series = CreateLineSeries();

                var units = SoilAnalysis.GetChemicalAnalysisUnits(propertyYName);
                if (!string.IsNullOrEmpty(units)) { SetAxis(series, units); }

                series.Values.AddRange(values);
                _SeriesInfo.Add(series, new SeriesInfo(well, "Análisis de suelos", propertyXName, displayNameX, propertyYName, displayNameY, GetValuesFromListName));
                series.Title = _SeriesInfo[series].ToString();
                return series;
            }
            return null;
        }

        protected ISeriesView CreateSeriesFromWaterAnalyses(Well well, string displayNameX, string displayNameY)
        {
            var propertyXName = WaterAnalysis.Properties[displayNameX].Name;
            var propertyYName = WaterAnalysis.Properties[displayNameY].Name;
            var values = FilterAndValidateValues(GetValues(well.WaterAnalyses, propertyXName, propertyYName), well, displayNameY);

            if (values != null)
            {
                var series = CreateLineSeries();

                var units = WaterAnalysis.GetChemicalAnalysisUnits(propertyYName);
                if (!string.IsNullOrEmpty(units)) { SetAxis(series, units); }

                series.Values.AddRange(values);
                _SeriesInfo.Add(series, new SeriesInfo(well, "Análisis de agua", propertyXName, displayNameX, propertyYName, displayNameY, GetValuesFromListName));
                series.Title = _SeriesInfo[series].ToString();
                return series;
            }
            return null;
        }

        protected ISeriesView CreateSeriesFromFlnaAnalyses(Well well, string displayNameX, string displayNameY)
        {
            var propertyXName = FlnaAnalysis.Properties[displayNameX].Name;
            var propertyYName = FlnaAnalysis.Properties[displayNameY].Name;
            var values = FilterAndValidateValues(GetValues(well.FlnaAnalyses, propertyXName, propertyYName), well, displayNameY);

            if (values != null)
            {
                var series = CreateLineSeries();

                var units = FlnaAnalysis.GetChemicalAnalysisUnits(propertyYName);
                if (!string.IsNullOrEmpty(units)) { SetAxis(series, units); }

                series.Values.AddRange(values);
                _SeriesInfo.Add(series, new SeriesInfo(well, "Análisis de FLNA", propertyXName, displayNameX, propertyYName, displayNameY, GetValuesFromListName));
                series.Title = _SeriesInfo[series].ToString();
                return series;
            }
            return null;
        }

        protected ISeriesView CreateSeriesFromPrecipitations()
        {
            var values = FilterAndValidateValues(GetValues(RepositoryWrapper.Instance.Precipitations.All, nameof(Precipitation.Date), nameof(Precipitation.Millimeters)), null, string.Empty);

            if (values != null)
            {
                var series = CreateColumnSeries();
                SetAxis(series, "mm");

                series.Values.AddRange(values);
                _SeriesInfo.Add(series, new SeriesInfo(null, "Precipitaciones", nameof(Precipitation.Date), "Fecha", 
                    nameof(Precipitation.Millimeters), "Milímetros", GetValuesFromListName));
                series.Title = _SeriesInfo[series].ToString();
                return series;
            }
            return null;
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
    }
}
