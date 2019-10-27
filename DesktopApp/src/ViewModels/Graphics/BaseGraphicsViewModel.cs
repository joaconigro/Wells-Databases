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
using Wells.CorePersistence.Repositories;
using Wells.CoreView.ViewInterfaces;
using Wells.CoreView.ViewModel;
using Wells.StandardModel.Models;
using Wells.View.Graphics;
using Wells.YPFModel;

namespace Wells.View.ViewModels
{
    public abstract class BaseGraphicsViewModel : BaseViewModel
    {
        protected IChartGraphicsView _Dialog;
        private DateTime _MinimunDate;
        private Random _RandomGenerator = new Random();
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
                aSeries.ScalesYAt = axis;
            else
            {
                var yAxis = new Axis() { Title = units, Position = AxisPosition.RightTop, LabelFormatter = YFormatter, FontSize = 12 };
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
                _Dialog.RemoveAxis(aSeries.ScalesYAt);
        }

        protected ISeriesView CreateSeriesFromMeasurements(Well well, string parameter)
        {
            var series = CreateLineSeries();
            series.Title = $"{well.Name} - {parameter}";

            var propertyName = Measurement.Properties[parameter].Name;
            var values = GetMeasurementsValues(well, propertyName, parameter);

            SetAxis(series, "metros");

            series.Values.AddRange(values);
            _SeriesInfo.Add(series, new SeriesInfo(well, propertyName, parameter, GetMeasurementsValues));
            return series;
        }

        List<DateModel> GetMeasurementsValues(Well well, string propertyName, string parameter)
        {
            var values = (from m in well.Measurements
                          let param = Convert.ToDouble(Interaction.CallByName(m, propertyName, CallType.Get))
                          where m.Date >= MinimunDate && m.Date <= MaximunDate && param != BusinessObject.NumericNullValue
                          orderby m.Date ascending
                          select new DateModel(m.Date, param)).ToList();

            if (values.Count < 2)
                throw new Exception($"Hay menos de dos datos de {parameter} para representar en el pozo {well.Name}, por lo tanto no se puede dibujar la línea.");
            return values;
        }

        protected ISeriesView CreateSeriesFromSoilAnalyses(Well well, string parameter)
        {
            var series = CreateLineSeries();
            series.Title = $"{well.Name} - {parameter}";


            var propertyName = SoilAnalysis.Properties[parameter].Name;
            var values = GetSoilAnalysesValues(well, propertyName, parameter);


            var units = SoilAnalysis.GetChemicalAnalysisUnits(propertyName);
            if (!string.IsNullOrEmpty(units))
                SetAxis(series, units);


            series.Values.AddRange(values);
            _SeriesInfo.Add(series, new SeriesInfo(well, propertyName, parameter, GetSoilAnalysesValues));
            return series;
        }


        List<DateModel> GetSoilAnalysesValues(Well well, string propertyName, string parameter)
        {
            var values = (from a in well.SoilAnalyses
                          let param = Convert.ToDouble(Interaction.CallByName(a, propertyName, CallType.Get))
                          where a.Date >= MinimunDate && a.Date <= MaximunDate && param != BusinessObject.NumericNullValue
                          orderby a.Date ascending
                          select new DateModel(a.Date, param)).ToList();

            if (values.Count < 2)
                throw new Exception($"Hay menos de dos datos de {parameter} para representar en el pozo {well.Name}, por lo tanto no se puede dibujar la línea.");
            return values;
        }


        protected ISeriesView CreateSeriesFromWaterAnalyses(Well well, string parameter)
        {
            var series = CreateLineSeries();
            series.Title = $"{well.Name} - {parameter}";


            var propertyName = WaterAnalysis.Properties[parameter].Name;
            var values = GetWaterAnalysesValues(well, propertyName, parameter);


            var units = WaterAnalysis.GetChemicalAnalysisUnits(propertyName);
            if (!string.IsNullOrEmpty(units))
                SetAxis(series, units);


            series.Values.AddRange(values);
            _SeriesInfo.Add(series, new SeriesInfo(well, propertyName, parameter, GetWaterAnalysesValues));
            return series;
        }


        List<DateModel> GetWaterAnalysesValues(Well well, string propertyName, string parameter)
        {
            var values = (from a in well.WaterAnalyses
                          let param = Convert.ToDouble(Interaction.CallByName(a, propertyName, CallType.Get))
                          where a.Date >= MinimunDate && a.Date <= MaximunDate && param != BusinessObject.NumericNullValue
                          orderby a.Date ascending
                          select new DateModel(a.Date, param)).ToList();

            if (values.Count < 2)
                throw new Exception($"Hay menos de dos datos de {parameter} para representar en el pozo {well.Name}, por lo tanto no se puede dibujar la línea.");
            return values;
        }


        protected ISeriesView CreateSeriesFromFLNAAnalyses(Well well, string parameter)
        {
            var series = CreateLineSeries();
            series.Title = $"{well.Name} - {parameter}";


            var propertyName = FLNAAnalysis.Properties[parameter].Name;
            var values = GetFLNAAnalysesValues(well, propertyName, parameter);


            var units = FLNAAnalysis.GetChemicalAnalysisUnits(propertyName);
            if (!string.IsNullOrEmpty(units))
                SetAxis(series, units);


            series.Values.AddRange(values);
            _SeriesInfo.Add(series, new SeriesInfo(well, propertyName, parameter, GetFLNAAnalysesValues));
            return series;
        }



        List<DateModel> GetFLNAAnalysesValues(Well well, string propertyName, string parameter)
        {
            var values = (from a in well.FLNAAnalyses
                          let param = Convert.ToDouble(Interaction.CallByName(a, propertyName, CallType.Get))
                          where a.Date >= MinimunDate && a.Date <= MaximunDate && param != BusinessObject.NumericNullValue
                          orderby a.Date ascending
                          select new DateModel(a.Date, param)).ToList();

            if (values.Count < 2)
                throw new Exception($"Hay menos de dos datos de {parameter} para representar en el pozo {well.Name}, por lo tanto no se puede dibujar la línea.");
            return values;
        }


        protected ISeriesView CreateSeriesFromPrecipitations()
        {
            var series = CreateColumnSeries();
            series.Title = "Precipitaciones";

            var values = GetPrecipitationsValues();
            SetAxis(series, "mm");

            series.Values.AddRange(values);
            _SeriesInfo.Add(series, new SeriesInfo(GetPrecipitationsValues));
            return series;
        }


        List<DateModel> GetPrecipitationsValues()
        {
            var values = (from p in RepositoryWrapper.Instance.Precipitations.All
                          where p.PrecipitationDate >= MinimunDate && p.PrecipitationDate <= MaximunDate
                          orderby p.PrecipitationDate ascending
                          select new DateModel(p.PrecipitationDate, p.Millimeters)).ToList();

            if (values.Count < 1)
                throw new Exception("No hay datos de precipitaciones para representar, por lo tanto no se dibujará la serie.");
            else if (values.Count > 200)
            {
                var aStep = Math.Max(1, values.Count / 150);
                var newValues = new List<DateModel>();
                for (int i = 0; i < values.Count; i += aStep)
                    newValues.Add(values[i]);
                return newValues;
            }

            return values;
        }


        LineSeries CreateLineSeries()
        {
            var seriesColor = Color.FromArgb(255, (byte)_RandomGenerator.Next(0, 255), (byte)_RandomGenerator.Next(0, 255), (byte)_RandomGenerator.Next(0, 255));

            var lineSeries = new LineSeries()
            {
                LineSmoothness = 0,
                Stroke = new SolidColorBrush(seriesColor),
                PointGeometrySize = 8,
                Fill = new SolidColorBrush(Colors.Transparent),
                Values = new ChartValues<DateModel>(),
                StrokeDashArray = new DoubleCollection() { 2.0 }
            };

            return lineSeries;
        }

        ColumnSeries CreateColumnSeries()
        {
            var seriesColor = Color.FromArgb(255, (byte)_RandomGenerator.Next(0, 255), (byte)_RandomGenerator.Next(0, 255), (byte)_RandomGenerator.Next(0, 255));

            var columnSeries = new ColumnSeries()
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
            public Func<Well, string, string, List<DateModel>> GetWellValuesFunc { get; }
            public Func<List<DateModel>> GetPrecipitationValuesFunc { get; }
            public bool IsFromWell { get; }

            public SeriesInfo(Well well, string propertyName, string parameterName, Func<Well, string, string, List<DateModel>> func)
            {
                Well = well;
                PropertyName = propertyName;
                ParameterName = parameterName;
                GetWellValuesFunc = func;
                GetPrecipitationValuesFunc = null;
                IsFromWell = true;
            }

            public SeriesInfo(Func<List<DateModel>> func)
            {
                Well = null;
                PropertyName = string.Empty;
                ParameterName = string.Empty;
                GetWellValuesFunc = null;
                GetPrecipitationValuesFunc = func;
                IsFromWell = false;
            }

            public List<DateModel> GetValues()
            {
                if (IsFromWell)
                    return GetWellValuesFunc.Invoke(Well, PropertyName, ParameterName);
                else
                    return GetPrecipitationValuesFunc.Invoke();
            }
        }
    }


}
