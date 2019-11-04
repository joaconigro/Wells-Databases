using LiveCharts;
using LiveCharts.Definitions.Series;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Wells.BaseView;
using Wells.BaseView.ViewInterfaces;
using Wells.Model;
using Wells.Persistence.Repositories;

namespace Wells.View.ViewModels
{
    public class CustomGraphicsViewModel : BaseGraphicsViewModel
    {
        private string selectedSeriesDataName;
        private string selectedParameterName;
        private int selectedFromOption;
        private string selectedWellName;
        private Well _Well;

        public List<string> SeriesDataNames => new List<string>() { "Mediciones", "Análisis de FLNA", "Análisis de agua", "Análisis de suelos" };
        public List<string> FromOptions => new List<string>() { "Pozos", "Precipitaciones" };

        public List<string> WellNames => RepositoryWrapper.Instance.Wells.Names;
        public bool ShowWellOptions => SelectedFromOption == 0 ? true : false;

        public int SelectedFromOption { get => selectedFromOption; set { SetValue(ref selectedFromOption, value); NotifyPropertyChanged(nameof(ShowWellOptions)); } }
        public string SelectedSeriesDataName { get => selectedSeriesDataName; set { SetValue(ref selectedSeriesDataName, value); NotifyPropertyChanged(nameof(Parameters)); } }
        public string SelectedParameterName { get => selectedParameterName; set { SetValue(ref selectedParameterName, value); } }

        public string SelectedWellName { get => selectedWellName; set { SetValue(ref selectedWellName, value); SetWell(); } }
        public List<string> Parameters
        {
            get
            {
                return selectedSeriesDataName switch
                {
                    "Mediciones" => Measurement.DoubleProperties.Keys.ToList(),
                    "Análisis de FLNA" => FLNAAnalysis.DoubleProperties.Keys.ToList(),
                    "Análisis de agua" => WaterAnalysis.DoubleProperties.Keys.ToList(),
                    _ => SoilAnalysis.DoubleProperties.Keys.ToList(),
                };
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

        protected override void SetValidators()
        {
            //Nothing to add yet.
        }

        public CustomGraphicsViewModel(IView view) : base(view)
        {
            Initialize();
        }

        void CreateSeries()
        {
            ISeriesView genericSeries = SelectedFromOption switch
            {
                0 => selectedSeriesDataName switch
                {
                    "Mediciones" => CreateSeriesFromMeasurements(_Well, SelectedParameterName),
                    "Análisis de FLNA" => CreateSeriesFromFLNAAnalyses(_Well, SelectedParameterName),
                    "Análisis de agua" => CreateSeriesFromWaterAnalyses(_Well, SelectedParameterName),
                    _ => CreateSeriesFromSoilAnalyses(_Well, SelectedParameterName),
                },

                _ => CreateSeriesFromPrecipitations(),
            };

            if (genericSeries != null)
            {
                SeriesCollection.Add(genericSeries);
            }
        }

        public ICommand CreateSeriesCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    CreateSeries();
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
                    SeriesCollection.Remove(SelectedSerie);
                    SelectedSerie = null;
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
