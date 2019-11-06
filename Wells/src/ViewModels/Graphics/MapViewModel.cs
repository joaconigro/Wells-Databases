using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using Wells.BaseView;
using Wells.BaseView.ViewInterfaces;
using Wells.BaseView.ViewModel;
using Wells.View.Graphics;
using Wells.Model;
using Microsoft.Maps.MapControl.WPF;
using System.Windows.Controls;
using System.Windows;

namespace Wells.View.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        private readonly Random _Random = new Random();
        IMapView _Dialog;
        private bool showLabels;
        private Pushpin selectedPushpin;
        private Location centerLocation;
        private double pointSize;
        private const double minPointSize = 2.0;
        private const double maxPointSize = 40.0;
        private string selectedClassName;
        private string selectedParameterName;

        public Location CenterLocation { get => centerLocation; set { SetValue(ref centerLocation, value); } }
        public List<Pushpin> Pushpins { get; }
        public Pushpin SelectedPushpin { get => selectedPushpin; set { SetValue(ref selectedPushpin, value); UpdateSelection(); } }
        public bool ShowLabels { get => showLabels; set { SetValue(ref showLabels, value); } }
        public List<string> ClassificationNames => new List<string>() {"Nada", "Mediciones", "Análisis de FLNA", "Análisis de agua", "Análisis de suelos" };
        public string SelectedClassName { get => selectedClassName; set { SetValue(ref selectedClassName, value); NotifyPropertyChanged(nameof(Parameters)); } }
        public string SelectedParameterName { get => selectedParameterName; set { SetValue(ref selectedParameterName, value); } }

        public List<string> Parameters
        {
            get
            {
                return selectedClassName switch
                {
                "Mediciones" => Measurement.DoubleProperties.Keys.ToList(),
                "Análisis de FLNA" => FlnaAnalysis.DoubleProperties.Keys.ToList(),
                "Análisis de agua" => WaterAnalysis.DoubleProperties.Keys.ToList(),
                "Análisis de suelos" => SoilAnalysis.DoubleProperties.Keys.ToList(),
                _ => new List<string>()
                };
            }

        }
        public double PointSize 
        { 
            get => pointSize; 
            set { 
                if(value >= minPointSize && value <= maxPointSize)
                {
                    SetValue(ref pointSize, value);
                    ChangePushpinAttributes();
                }
            } 
        }


        public MapViewModel(IEnumerable<Well> wells) : base(null)
        {
            Pushpins = new List<Pushpin>();
            showLabels = true;
            Initialize();
            InitializeData(wells);
        }


        void InitializeData(IEnumerable<Well> wells)
        {
            foreach (var w in wells.Where(well => well.HasGeographic))
            {
                var p = new Pushpin()
                {
                    Name = w.Name,
                    Location = new Location(w.Latitude, w.Longitude),
                    ToolTip = w.Name,
                    Template = Application.Current.FindResource("PushpinDefaultTemplate") as ControlTemplate,
                    Background = new SolidColorBrush(GetRandomColor())
                };
                p.MouseDown += new MouseButtonEventHandler(PushpinMouseDown);
                Pushpins.Add(p);
            }

            var lat = wells.Where(well => well.HasGeographic).Average(w => w.Latitude);
            var lon = wells.Where(well => well.HasGeographic).Average(w => w.Longitude);
            CenterLocation = new Location(lat, lon);

            PointSize = 10.0;
        }

        void ChangePushpinAttributes()
        {
            foreach (Pushpin p in Pushpins)
            {
                p.Width = PointSize;
            }
            if (SelectedPushpin != null)
            {
                SelectedPushpin.Width = PointSize + 2;
            }
        }

        void PushpinMouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectedPushpin = (Pushpin)sender;
            UpdateSelection();
        }

        void UpdateSelection()
        {
            foreach (Pushpin p in Pushpins)
            {
                p.Template = Application.Current.FindResource("PushpinDefaultTemplate") as ControlTemplate;
            }
            if (SelectedPushpin != null)
            {
                SelectedPushpin.Template = Application.Current.FindResource("PushpinSelectedTemplate") as ControlTemplate;
            }
        }

        Color GetRandomColor()
        {
            var names = (IList)Enum.GetValues(typeof(System.Drawing.KnownColor));
            var randomColorName = (System.Drawing.KnownColor)names[_Random.Next(names.Count)];
            var randomColor = System.Drawing.Color.FromKnownColor(randomColorName);
            return Color.FromArgb(randomColor.A, randomColor.R, randomColor.G, randomColor.B);
        }
        protected override void SetValidators()
        {
            //No need to implement yet.
        }

        protected override void SetCommandUpdates()
        {
            Add(nameof(SelectedPushpin), ChangeColorCommand);
        }



        protected override void OnSetView(IView view)
        {
            base.OnSetView(view);
            _Dialog = (IMapView)view;
        }




        public ICommand SaveImageCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    _Dialog.SaveImage("Mapa");
                }, (obj) => true, OnError);
            }
        }

        public ICommand ChangeColorCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    var brush = SelectedPushpin.Background as SolidColorBrush;
                    var currentColor = System.Drawing.Color.FromArgb(brush.Color.A,
                                                                     brush.Color.R,
                                                                     brush.Color.G,
                                                                     brush.Color.B);
                    var color = SharedBaseView.ShowColorDialog(currentColor);
                    SelectedPushpin.Background = new SolidColorBrush(color);
                }, (obj) => SelectedPushpin != null, OnError);
            }
        }

    }
}
