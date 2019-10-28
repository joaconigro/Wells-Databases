using LiveCharts;
using LiveCharts.Definitions.Series;
using System;
using System.Windows.Input;
using Wells.CoreView;
using Wells.CoreView.ViewInterfaces;
using Wells.View.Graphics;
using Wells.YPFModel;

namespace Wells.View.ViewModels
{
    public class PremadeGraphicsViewModel : BaseGraphicsViewModel
    {
        private Well _Well;
        PremadeSeriesInfoCollection _GraphicSeries;
        public string Title { get; }
        protected override void SetCommandUpdates() { }

        protected override void SetValidators() { }

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
                    switch (gs.ParameterGroup)
                    {
                        case "Mediciones":
                            genericSeries = CreateSeriesFromMeasurements(_Well, gs.PropertyDisplayName);
                            break;
                        case "Análisis de FLNA":
                            genericSeries = CreateSeriesFromFLNAAnalyses(_Well, gs.PropertyDisplayName);
                            break;
                        case "Análisis de agua":
                            genericSeries = CreateSeriesFromWaterAnalyses(_Well, gs.PropertyDisplayName);
                            break;
                        default:
                            genericSeries = CreateSeriesFromSoilAnalyses(_Well, gs.PropertyDisplayName);
                            break;
                    }
                }
                else
                {
                    genericSeries = CreateSeriesFromPrecipitations();
                }

                if (genericSeries != null)
                    SeriesCollection.Add(genericSeries);
            }
        }

        public ICommand SaveChartImageCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    _Dialog.SaveImage();
                }, (obj) => true, OnError);
            }
        }
    }
}
