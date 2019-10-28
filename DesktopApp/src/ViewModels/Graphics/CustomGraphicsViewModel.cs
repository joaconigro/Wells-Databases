using LiveCharts;
using LiveCharts.Definitions.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Wells.CorePersistence.Repositories;
using Wells.CoreView;
using Wells.CoreView.ViewInterfaces;
using Wells.YPFModel;

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
                switch (selectedSeriesDataName)
                {
                    case "Mediciones":
                        return Measurement.DoubleProperties.Keys.ToList();
                    case "Análisis de FLNA":
                        return FLNAAnalysis.DoubleProperties.Keys.ToList();
                    case "Análisis de agua":
                        return WaterAnalysis.DoubleProperties.Keys.ToList();
                    default:
                        return SoilAnalysis.DoubleProperties.Keys.ToList();
                }
            }

        }

        void SetWell()
        {
            if (!string.IsNullOrEmpty(selectedWellName))
                _Well = RepositoryWrapper.Instance.Wells.FindByName(selectedWellName);
            else
                _Well = null;
        }

        protected override void SetCommandUpdates()
        {
            Add(nameof(SelectedSerie), RemoveSeriesCommand);
        }

        protected override void SetValidators() { }

        public CustomGraphicsViewModel(IView view) : base(view)
        {
            Initialize();
        }

        void CreateSeries()
        {
            ISeriesView genericSeries = null;

            switch (SelectedFromOption)
            {
                case 0:
                    switch (selectedSeriesDataName)
                    {
                        case "Mediciones":
                            genericSeries = CreateSeriesFromMeasurements(_Well, SelectedParameterName);
                            break;
                        case "Análisis de FLNA":
                            genericSeries = CreateSeriesFromFLNAAnalyses(_Well, SelectedParameterName);
                            break;
                        case "Análisis de agua":
                            genericSeries = CreateSeriesFromWaterAnalyses(_Well, SelectedParameterName);
                            break;
                        default:
                            genericSeries = CreateSeriesFromSoilAnalyses(_Well, SelectedParameterName);
                            break;
                    }
                    break;

                case 1:
                    genericSeries = CreateSeriesFromPrecipitations();
                    break;
            }

            if (genericSeries != null)
                SeriesCollection.Add(genericSeries);
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
                    _Dialog.SaveImage();
                }, (obj) => true, OnError);
            }
        }
    }
}
