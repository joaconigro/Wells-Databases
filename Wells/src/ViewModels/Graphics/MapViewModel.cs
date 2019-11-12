﻿using Microsoft.Maps.MapControl.WPF;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Wells.BaseView;
using Wells.BaseView.ViewInterfaces;
using Wells.BaseView.ViewModel;
using Wells.Model;
using Wells.View.Graphics;
using Wells.View.UserControls;

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
        private double minPointSize;
        private double maxPointSize;
        private const double lowerPointSize = 5.0;
        private const double upperPointSize = 40.0;
        private string selectedClass;
        private string selectedParameter;
        private Gradient selectedGradient;
        private bool isClassified;
        private string selectedFunction;
        private readonly List<Well> wells;

        public Location CenterLocation { get => centerLocation; set { SetValue(ref centerLocation, value); } }
        public List<Pushpin> Pushpins { get; }
        public List<Gradient> Gradients { get; private set; }
        public Pushpin SelectedPushpin { get => selectedPushpin; set { SetValue(ref selectedPushpin, value); UpdateSelection(); } }
        public bool ShowLabels { get => showLabels; set { SetValue(ref showLabels, value); } }
        public List<string> ClassificationNames => new List<string> { "Nada", "Mediciones", "Análisis de FLNA", "Análisis de agua", "Análisis de suelos" };
        public List<string> Functions => new List<string> { "Máximo", "Mínimo", "Promedio", "Última fecha" };
        public string SelectedClass { get => selectedClass; set { SetValue(ref selectedClass, value); NotifyPropertyChanged(nameof(Parameters)); } }
        public string SelectedFunction { get => selectedFunction; set { SetValue(ref selectedFunction, value); ChangePushpinAttributes(); } }
        public string SelectedParameter { get => selectedParameter; set { SetValue(ref selectedParameter, value); ChangePushpinAttributes(); } }
        public Gradient SelectedGradient { get => selectedGradient; set { SetValue(ref selectedGradient, value); ChangePushpinAttributes(); } }
        public bool IsClassified { get => isClassified; set { SetValue(ref isClassified, value); NotifyPropertyChanged(nameof(IsNotClassified)); ChangePushpinAttributes(); } }
        public bool IsNotClassified => !IsClassified;
        public List<string> Parameters
        {
            get
            {
                selectedParameter = string.Empty;
                IsClassified = selectedClass != "Nada";
                return selectedClass switch
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
            set
            {
                if (value >= lowerPointSize && value <= upperPointSize)
                {
                    SetValue(ref pointSize, value);
                    ChangePushpinAttributes();
                }
            }
        }

        public double MinPointSize
        {
            get => minPointSize;
            set
            {
                if (value >= lowerPointSize && value <= maxPointSize)
                {
                    SetValue(ref minPointSize, value);
                    ChangePushpinAttributes();
                }
            }
        }

        public double MaxPointSize
        {
            get => maxPointSize;
            set
            {
                if (value >= minPointSize && value <= upperPointSize)
                {
                    SetValue(ref maxPointSize, value);
                    ChangePushpinAttributes();
                }
            }
        }


        public MapViewModel(IEnumerable<Well> wells) : base(null)
        {
            Gradients = ManageColorMapsViewModel.ReadGradients();
            this.wells = wells.Where(w => w.HasGeographic).OrderBy(w => w.Name).ToList();
            Pushpins = new List<Pushpin>();
            showLabels = true;
            Initialize();
            InitializeData();

            isClassified = false;
            selectedClass = "Nada";
            selectedGradient = Gradients.First();

            var lat = this.wells.Average(w => w.Latitude);
            var lon = this.wells.Average(w => w.Longitude);
            CenterLocation = new Location(lat, lon);
            PointSize = 13.0;
            minPointSize = lowerPointSize;
            maxPointSize = upperPointSize;
        }


        void InitializeData()
        {
            foreach (var w in wells)
            {
                var p = new Pushpin
                {
                    Name = w.Name,
                    Location = new Location(w.Latitude, w.Longitude),
                    Template = Application.Current.FindResource("PushpinDefaultTemplate") as ControlTemplate,
                    Background = new SolidColorBrush(SharedBaseView.GetRandomColor(_Random))
                };
                p.MouseDown += new MouseButtonEventHandler(PushpinMouseDown);
                Pushpins.Add(p);
            }

            CreateTooltips();
        }

        void ChangePushpinAttributes()
        {
            if (IsClassified && !string.IsNullOrEmpty(SelectedParameter) && SelectedGradient != null)
            {
                foreach (Pushpin p in Pushpins)
                {
                    var well = wells.First(w => w.Name == p.Name);
                    p.Tag = GetValue(well);
                }
                var minValue = Pushpins.Min(p => (double)p.Tag);
                var maxValue = Pushpins.Max(p => (double)p.Tag);

                foreach (Pushpin p in Pushpins)
                {
                    if (minPointSize.Equals(maxPointSize))
                    {
                        p.Width = minPointSize;
                    }
                    else
                    {
                        p.Width = minPointSize + MapTo((double)p.Tag, minValue, maxValue, minPointSize, maxPointSize);
                    }

                    var offset = MapTo((double)p.Tag, minValue, maxValue, 0.0, 1.0);
                    p.Background = new SolidColorBrush(SelectedGradient.GetColor(offset));
                }
            }
            else
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
            CreateTooltips();
        }

        void CreateTooltips()
        {
            if (IsClassified && !string.IsNullOrEmpty(SelectedParameter))
            {
                foreach (Pushpin p in Pushpins)
                {
                    var info = $"{selectedParameter} = { Convert.ToDouble(p.Tag).ToString("N2")}";
                    p.ToolTip = new CustomTooltip(p.Name, info);
                }
            }
            else
            {
                foreach (Pushpin p in Pushpins)
                {
                    p.ToolTip = new CustomTooltip(p.Name);
                }
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

        public void ClearSelection()
        {
            SelectedPushpin = null;
            UpdateSelection();
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

        private double MapTo(double value, double minValue, double maxValue, double min, double max)
        {
            return (value - minValue) * (max - min) / (maxValue - minValue);
        }

        double GetValue(Well well)
        {
            try
            {
                if (SelectedClass == "Mediciones")
                {
                    var propName = Measurement.DoubleProperties[SelectedParameter].Name;
                    return well.Measurements.Max(m => (double)Interaction.CallByName(m, propName, CallType.Get));
                }
                else if (SelectedClass == "Análisis de FLNA")
                {
                    var propName = FlnaAnalysis.DoubleProperties[SelectedParameter].Name;
                    return well.FlnaAnalyses.Max(m => (double)Interaction.CallByName(m, propName, CallType.Get));
                }
                else if (SelectedClass == "Análisis de agua")
                {
                    var propName = WaterAnalysis.DoubleProperties[SelectedParameter].Name;
                    return well.WaterAnalyses.Max(m => (double)Interaction.CallByName(m, propName, CallType.Get));
                }
                else if (SelectedClass == "Análisis de suelos")
                {
                    var propName = SoilAnalysis.DoubleProperties[SelectedParameter].Name;
                    return well.SoilAnalyses.Max(m => (double)Interaction.CallByName(m, propName, CallType.Get));
                }
                else { return 0.0; }
            }
            catch
            {
                ShowErrorMessage("Uno o más de los pozos utilizados no tiene los datos seleccionados.");
                return 0.0;
            }
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
                    var currentColor = brush.Color.ToDrawingColor();
                    var color = SharedBaseView.ShowColorDialog(currentColor);
                    SelectedPushpin.Background = new SolidColorBrush(color);
                }, (obj) => SelectedPushpin != null, OnError);
            }
        }

        public ICommand ManageColorMapsCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    var gradName = SelectedGradient?.Name;
                    if (_Dialog.ShowManageColorMapDialog())
                    {
                        Gradients = ManageColorMapsViewModel.ReadGradients();
                        NotifyPropertyChanged(nameof(Gradients));
                        if (!string.IsNullOrEmpty(gradName))
                        {
                            SelectedGradient = Gradients.FirstOrDefault(g => g.Name == gradName);
                        }
                    }
                }, (obj) => true, OnError);
            }
        }

    }
}
