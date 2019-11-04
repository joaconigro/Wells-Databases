using LiveCharts;
using LiveCharts.Definitions.Series;
using System;
using System.Windows.Input;
using Wells.BaseView;
using Wells.BaseView.ViewInterfaces;
using Wells.Model;
using Wells.View.Graphics;

namespace Wells.View.ViewModels
{
    public class PremadeGraphicsViewModel : BaseGraphicsViewModel
    {
        private readonly Well _Well;
        readonly PremadeSeriesInfoCollection _GraphicSeries;
        public string Title { get; }
        protected override void SetCommandUpdates()
        {
            //No need to implement yet.
        }

        protected override void SetValidators()
        {
            //No need to implement yet.
        }

        public PremadeGraphicsViewModel(IView view, Well well, PremadeSeriesInfoCollection series) : base(view)
        {
            MinimunDate = new DateTime(2000, 1, 1);
            Initialize();
            _Well = well;
            Title = well.Name;
            _GraphicSeries = series;
            CreateSeries();
        }


        void CreateSeries()
        {
            foreach (var gs in _GraphicSeries.Values)
            {
                ISeriesView genericSeries = null;

                if (gs.IsFromWell)
                {
                    genericSeries = gs.ParameterGroup switch
                    {
                        "Mediciones" => CreateSeriesFromMeasurements(_Well, gs.PropertyDisplayName),
                        "Análisis de FLNA" => CreateSeriesFromFLNAAnalyses(_Well, gs.PropertyDisplayName),
                        "Análisis de agua" => CreateSeriesFromWaterAnalyses(_Well, gs.PropertyDisplayName),
                        _ => CreateSeriesFromSoilAnalyses(_Well, gs.PropertyDisplayName),
                    };
                }
                else
                {
                    genericSeries = CreateSeriesFromPrecipitations();
                }

                if (genericSeries != null)
                {
                    SeriesCollection.Add(genericSeries);
                }
            }
        }

        public ICommand SaveChartImageCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    _Dialog.SaveImage(_Well.Name);
                }, (obj) => true, OnError);
            }
        }
    }
}
