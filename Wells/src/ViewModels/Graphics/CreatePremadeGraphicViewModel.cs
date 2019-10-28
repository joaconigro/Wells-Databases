using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Xml.Serialization;
using Wells.BaseView;
using Wells.BaseView.Validators;
using Wells.BaseView.ViewInterfaces;
using Wells.BaseView.ViewModel;
using Wells.View.Graphics;
using Wells.Model;

namespace Wells.View.ViewModels
{
    public class CreatePremadeGraphicViewModel : BaseViewModel
    {
        private string title;
        private string selectedSeriesDataName;
        private string selectedParameterName;
        private int selectedFromOption;
        private PremadeSeriesInfo selectedSerie;
        private PremadeSeriesInfoCollection selectedGraphic;
        protected override void SetCommandUpdates()
        {
            Add(nameof(Title), new List<ICommand>() { CreateGraphicCommand });
            Add(nameof(SelectedSerie), new List<ICommand>() { RemoveSeriesCommand });
            Add(nameof(SelectedGraphic), new List<ICommand>() { RemoveGraphicCommand });
        }

        protected override void SetValidators()
        {
            Add(nameof(Title), new EmptyStringValidator("Título"));
        }

        public List<string> SeriesDataNames => new List<string>() { "Mediciones", "Análisis de FLNA", "Análisis de agua", "Análisis de suelos" };
        public List<string> FromOptions => new List<string>() { "Pozos", "Precipitaciones" };

        public string Title { get => title; set { SetValue(ref title, value); } }
        public string SelectedSeriesDataName { get => selectedSeriesDataName; set { SetValue(ref selectedSeriesDataName, value); NotifyPropertyChanged(nameof(Parameters)); } }

        public string SelectedParameterName { get => selectedParameterName; set { SetValue(ref selectedParameterName, value); } }

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

        public bool ShowWellOptions => SelectedFromOption == 0 ? true : false;

        public int SelectedFromOption { get => selectedFromOption; set { SetValue(ref selectedFromOption, value); NotifyPropertyChanged(nameof(ShowWellOptions)); } }

        public PremadeSeriesInfo SelectedSerie { get => selectedSerie; set { SetValue(ref selectedSerie, value); RaiseCommandUpdates(nameof(SelectedSerie)); } }
        public PremadeSeriesInfoCollection SelectedGraphic { get => selectedGraphic; set { SetValue(ref selectedGraphic, value); RaiseCommandUpdates(nameof(SelectedSerie)); } }

        PremadeSeriesInfoCollection _NewCollection;
        public ObservableCollection<PremadeSeriesInfo> Series { get; }
        public ObservableCollection<PremadeSeriesInfoCollection> Graphics { get; }




        public CreatePremadeGraphicViewModel(IView view) : base(view)
        {
            Initialize();
            _NewCollection = new PremadeSeriesInfoCollection();
            Series = new ObservableCollection<PremadeSeriesInfo>(); 
            var values = ReadPremadeGraphics();
            if (values != null && values.Any())
                Graphics = new ObservableCollection<PremadeSeriesInfoCollection>(values);
            else
                Graphics = new ObservableCollection<PremadeSeriesInfoCollection>();
        }


        public static List<PremadeSeriesInfoCollection> ReadPremadeGraphics()
        {
            var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "WellManager");
            var filename = Path.Combine(dir, "PremadeGraphics.wpg");

            if (File.Exists(filename))
            {
                var serializer = new XmlSerializer(typeof(List<PremadeSeriesInfoCollection>));
                var entities = new List<PremadeSeriesInfoCollection>();
                using (var reader = new StreamReader(filename))
                {
                    entities = (List<PremadeSeriesInfoCollection>)serializer.Deserialize(reader);
                }

                return entities;
            }
            return new List<PremadeSeriesInfoCollection>();
        }

        void SavePremadeGraphics()
        {
            var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "WellManager");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var filename = Path.Combine(dir, "PremadeGraphics.wpg");

            var serializer = new XmlSerializer(typeof(List<PremadeSeriesInfoCollection>));
            var entities = Graphics.ToList();
            using (var writer = new StreamWriter(filename))
            {
                serializer.Serialize(writer, entities);
            }

        }


        public ICommand CreateSeriesCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    PremadeSeriesInfo s;
                    if (ShowWellOptions)
                    {
                        s = new PremadeSeriesInfo()
                        {
                            IsFromWell = ShowWellOptions,
                            ParameterGroup = SelectedSeriesDataName,
                            PropertyDisplayName = SelectedParameterName
                        };
                    }
                    else
                    {
                        s = new PremadeSeriesInfo() { IsFromWell = ShowWellOptions };
                    }
                    Series.Add(s);
                    (CreateGraphicCommand as RelayCommand).OnCanExecuteChanged();
                }, (obj) => true, OnError);
            }
        }


        public ICommand RemoveSeriesCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    Series.Remove(SelectedSerie);
                    SelectedSerie = null;
                    (CreateGraphicCommand as RelayCommand).OnCanExecuteChanged();
                }, (obj) => SelectedSerie != null, OnError);
            }
        }

        public ICommand CreateGraphicCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    _NewCollection.Title = Title;
                    _NewCollection.Values = Series.ToList();
                    Graphics.Add(_NewCollection);
                    Series.Clear();
                    _NewCollection = new PremadeSeriesInfoCollection();
                    SavePremadeGraphics();
                }, (obj) => !string.IsNullOrEmpty(Title) && Series.Any(), OnError);
            }
        }


        public ICommand RemoveGraphicCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    Graphics.Remove(SelectedGraphic);
                    SelectedGraphic = null;
                    SavePremadeGraphics();
                }, (obj) => SelectedGraphic != null, OnError);
            }
        }

    }
}
