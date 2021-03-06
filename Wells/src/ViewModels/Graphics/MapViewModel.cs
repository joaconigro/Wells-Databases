﻿using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Wells.BaseModel.Models;
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
        private CustomPushpin selectedPushpin;
        private double pointSize;
        private double minPointSize;
        private double maxPointSize;
        private const double lowerPointSize = 5.0;
        private const double upperPointSize = 50.0;
        private string selectedClass;
        private string selectedParameter;
        private Gradient selectedGradient;
        private bool isClassified;
        private string selectedFunction;
        private double mapRotation;
        private readonly List<Well> wells;

        public List<CustomPushpin> Pushpins { get; }
        public List<Gradient> Gradients { get; private set; }
        public double MapRotation { get => mapRotation; set { SetValue(ref mapRotation, value); _Dialog.UpdateHeading(MapRotation); } }
        public CustomPushpin SelectedPushpin { get => selectedPushpin; set { SetValue(ref selectedPushpin, value); UpdateSelection(); } }
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
                var list = selectedClass switch
                {
                    "Mediciones" => Measurement.DoubleProperties.Keys.ToList(),
                    "Análisis de FLNA" => FlnaAnalysis.DoubleProperties.Keys.ToList(),
                    "Análisis de agua" => WaterAnalysis.DoubleProperties.Keys.ToList(),
                    "Análisis de suelos" => SoilAnalysis.DoubleProperties.Keys.ToList(),
                    _ => new List<string>()
                };
                if (list.Any())
                {
                    SelectedParameter = list.First();
                }
                return list;
            }
        }

        public bool ShowLabels
        {
            get => showLabels;
            set
            {
                SetValue(ref showLabels, value);
                UpdateLabels();
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
            Pushpins = new List<CustomPushpin>();
            showLabels = true;
            Initialize();
            InitializeData();

            isClassified = false;
            selectedClass = "Nada";
            selectedFunction = "Máximo";
            selectedGradient = Gradients.First();

            PointSize = 13.0;
            minPointSize = lowerPointSize;
            maxPointSize = upperPointSize;
        }


        void InitializeData()
        {
            var template = Application.Current.FindResource("CustomPushpinTemplate") as ControlTemplate;
            var brush = new SolidColorBrush(SharedBaseView.GetRandomColor(_Random));
            foreach (var w in wells)
            {
                var p = new CustomPushpin(w, template) { Background = brush };
                p.MouseDown += new MouseButtonEventHandler(PushpinMouseDown);
                Pushpins.Add(p);
            }

            CreateTooltips();
        }

        void ChangePushpinAttributes()
        {
            if (IsClassified && !string.IsNullOrEmpty(SelectedParameter) && SelectedGradient != null)
            {
                foreach (CustomPushpin p in Pushpins)
                {
                    var well = wells.First(w => w.Name == p.Name);
                    p.Value = GetValue(well);
                }
                var minValue = Pushpins.Min(p => p.Value);
                var maxValue = Pushpins.Max(p => p.Value);

                foreach (CustomPushpin p in Pushpins)
                {
                    if (minPointSize.Equals(maxPointSize))
                    {
                        p.Width = minPointSize;
                    }
                    else
                    {
                        p.Width = minPointSize + MapTo(p.Value, minValue, maxValue, minPointSize, maxPointSize);
                    }

                    var offset = MapTo(p.Value, minValue, maxValue, 0.0, 1.0);
                    p.Background = new SolidColorBrush(SelectedGradient.GetColor(offset));
                }
            }
            else
            {
                foreach (CustomPushpin p in Pushpins)
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
                foreach (CustomPushpin p in Pushpins)
                {
                    var info = $"{selectedParameter} = { p.Value.ToString("N2")}";
                    p.ToolTip = new CustomTooltip(p.Label, info);
                }
            }
            else
            {
                foreach (CustomPushpin p in Pushpins)
                {
                    p.ToolTip = new CustomTooltip(p.Label);
                }
            }
        }

        void PushpinMouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectedPushpin = (CustomPushpin)sender;
            UpdateSelection();
        }

        void UpdateSelection()
        {
            foreach (CustomPushpin p in Pushpins)
            {
                p.IsSelected = false;
            }
            if (SelectedPushpin != null)
            {
                SelectedPushpin.IsSelected = true;
            }
        }

        void UpdateLabels()
        {
            foreach (CustomPushpin p in Pushpins)
            {
                p.ShowLabel = showLabels;
            }
        }

        public void ClearSelection()
        {
            SelectedPushpin = null;
            UpdateSelection();
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
                    return GetValue(well.Measurements, propName);
                }
                else if (SelectedClass == "Análisis de FLNA")
                {
                    var propName = FlnaAnalysis.DoubleProperties[SelectedParameter].Name;
                    return GetValue(well.FlnaAnalyses, propName);
                }
                else if (SelectedClass == "Análisis de agua")
                {
                    var propName = WaterAnalysis.DoubleProperties[SelectedParameter].Name;
                    return GetValue(well.WaterAnalyses, propName);
                }
                else if (SelectedClass == "Análisis de suelos")
                {
                    var propName = SoilAnalysis.DoubleProperties[SelectedParameter].Name;
                    return GetValue(well.SoilAnalyses, propName);
                }
                else { return 0.0; }
            }
            catch
            {
                return 0.0;
            }
        }

        double GetValue(IEnumerable<IBusinessObject> objects, string propertyName)
        {
            if ((bool)!objects?.Any()) { return 0.0; }

            if (SelectedFunction == "Máximo")
            {
                return objects.Max(m => (double)Interaction.CallByName(m, propertyName, CallType.Get));
            }
            else if (SelectedFunction == "Mínimo")
            {
                return objects.Min(m => (double)Interaction.CallByName(m, propertyName, CallType.Get));
            }
            else if (SelectedFunction == "Promedio")
            {
                return objects.Average(m => (double)Interaction.CallByName(m, propertyName, CallType.Get));
            }
            else if (SelectedFunction == "Última fecha")
            {
                var obj = objects.OrderBy(o => (DateTime)Interaction.CallByName(o, "Date", CallType.Get)).Last();
                return (double)Interaction.CallByName(obj, propertyName, CallType.Get);
            }
            else { return 0.0; }
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
                    if (SelectedPushpin != null)
                    {
                        var brush = SelectedPushpin.Background as SolidColorBrush;
                        var currentColor = brush.Color.ToDrawingColor();
                        var color = SharedBaseView.ShowColorDialog(currentColor);
                        SelectedPushpin.Background = new SolidColorBrush(color);
                    }
                    else
                    {
                        var brush = Pushpins.First().Background as SolidColorBrush;
                        var currentColor = brush.Color.ToDrawingColor();
                        var color = SharedBaseView.ShowColorDialog(currentColor);
                        foreach (var p in Pushpins)
                        {
                            p.Background = new SolidColorBrush(color);
                        }
                    }

                }, (obj) => true, OnError);
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
