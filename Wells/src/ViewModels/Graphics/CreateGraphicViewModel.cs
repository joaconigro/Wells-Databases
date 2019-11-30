using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using Wells.BaseView;
using Wells.BaseView.Validators;
using Wells.BaseView.ViewInterfaces;
using Wells.BaseView.ViewModel;
using Wells.Model;
using Wells.Resources;
using Wells.View.Graphics;

namespace Wells.View.ViewModels
{
    public class CreateGraphicViewModel : BaseViewModel
    {
        private string title;
        private string selectedSeriesDataName;
        private string selectedXParameter;
        private string selectedYParameter;
        private int selectedFromOption;
        private SeriesInfo selectedSerie;
        private SeriesInfoCollection selectedGraphic;
        private readonly Random randomGenerator = new Random();

        protected override void SetCommandUpdates()
        {
            Add(nameof(Title), CreateGraphicCommand);
            Add(nameof(SelectedSerie), RemoveSeriesCommand);
            Add(nameof(SelectedGraphic), RemoveGraphicCommand);
        }

        protected override void SetValidators()
        {
            Add(nameof(Title), new EmptyStringValidator("Título"));
        }

        public List<string> SeriesDataNames => new List<string> { "Mediciones", "Análisis de agua" };
        public List<string> FromOptions => new List<string> { "Pozos", "Precipitaciones" };

        public string Title { get => title; set { SetValue(ref title, value); } }
        public string SelectedYParameter { get => selectedYParameter; set { SetValue(ref selectedYParameter, value); } }
        public string SelectedXParameter { get => selectedXParameter; set { SetValue(ref selectedXParameter, value); } }

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
                return (bool)Series?.Any() ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public bool AreDateOptionsEnabled
        {
            get
            {
                bool result = true;
                if ((bool)Series?.Any())
                {
                    result = Series.First().IsDateBased;
                }
                return result;
            }
        }

        public bool ShowWellOptions => SelectedFromOption == 0 ? true : false;

        public int SelectedFromOption { get => selectedFromOption; set { SetValue(ref selectedFromOption, value); NotifyPropertyChanged(nameof(ShowWellOptions)); } }

        public SeriesInfo SelectedSerie { get => selectedSerie; set { SetValue(ref selectedSerie, value); RaiseCommandUpdates(nameof(SelectedSerie)); } }
        public SeriesInfoCollection SelectedGraphic { get => selectedGraphic; set { SetValue(ref selectedGraphic, value); RaiseCommandUpdates(nameof(SelectedSerie)); } }

        SeriesInfoCollection _NewCollection;
        public ObservableCollection<SeriesInfo> Series { get; }
        public ObservableCollection<SeriesInfoCollection> Graphics { get; }




        public CreateGraphicViewModel(IView view) : base(view)
        {
            Initialize();
            _NewCollection = new SeriesInfoCollection();
            Series = new ObservableCollection<SeriesInfo>();
            var values = ReadPremadeGraphics();
            if (values != null && values.Any())
            {
                Graphics = new ObservableCollection<SeriesInfoCollection>(values);
            }
            else
            {
                Graphics = new ObservableCollection<SeriesInfoCollection>();
            }
        }

        void OnSeriesCollectionChanged()
        {
            if ((bool)Series?.Any() && AreDateOptionsEnabled)
            {
                SelectedXParameter = "Fecha";
            }
        }

        public static List<SeriesInfoCollection> ReadPremadeGraphics()
        {
            var filename = Path.Combine(AppSettings.SettingsDirectory, "Graphics.wpg");
            var entities = new List<SeriesInfoCollection>();

            if (File.Exists(filename))
            {
                var serializer = new XmlSerializer(typeof(List<SeriesInfoCollection>));
                using var reader = new StreamReader(filename);
                entities = (List<SeriesInfoCollection>)serializer.Deserialize(reader);

            }
            return entities;
        }

        void SavePremadeGraphics()
        {
            var filename = Path.Combine(AppSettings.SettingsDirectory, "Graphics.wpg");

            var serializer = new XmlSerializer(typeof(List<SeriesInfoCollection>));
            var entities = Graphics.ToList();
            using var writer = new StreamWriter(filename);
            serializer.Serialize(writer, entities);
        }


        public ICommand CreateSeriesCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    SeriesInfo s;
                    if (ShowWellOptions)
                    {
                        s = new SeriesInfo(ShowWellOptions, SelectedSeriesDataName, selectedXParameter, selectedYParameter);
                    }
                    else
                    {
                        s = new SeriesInfo(ShowWellOptions);
                    }
                    s.Color = SharedBaseView.GetRandomColor(randomGenerator);
                    Series.Add(s);
                    OnSeriesCollectionChanged();
                    NotifyPropertyChanged(nameof(AreDateOptionsEnabled));
                    NotifyPropertyChanged(nameof(XParametersVisibility));
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
                    OnSeriesCollectionChanged();
                    NotifyPropertyChanged(nameof(AreDateOptionsEnabled));
                    NotifyPropertyChanged(nameof(XParametersVisibility));
                    (CreateGraphicCommand as RelayCommand).OnCanExecuteChanged();
                }, (obj) => SelectedSerie != null, OnError);
            }
        }

        public ICommand ChangeColorSeriesCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    SelectedSerie.Color = SharedBaseView.ShowColorDialog(SelectedSerie.Color.ToDrawingColor());
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
                    _NewCollection = new SeriesInfoCollection();
                    SavePremadeGraphics();
                    NotifyPropertyChanged(nameof(AreDateOptionsEnabled));
                    NotifyPropertyChanged(nameof(XParametersVisibility));
                    Title = string.Empty;
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
