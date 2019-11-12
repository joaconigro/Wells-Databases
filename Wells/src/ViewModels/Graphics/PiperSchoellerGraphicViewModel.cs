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

namespace Wells.View.ViewModels
{
    public class PiperSchoellerGraphicViewModel : BaseViewModel
    {
        private const double CalciumMeq = 20.0;
        private const double PotassiumMeq = 39.0;
        private const double SodiumMeq = 23.0;
        private const double MagnesiumMeq = 12.15;
        private const double BicarbonateMeq = 61.0;
        private const double SulfatesMeq = 48.0;
        private const double ChloridesMeq = 35.5;

        private readonly Random _Random = new Random();

        IPiperSchoellerGraphicView _Dialog;

        private bool showZones;
        private int pointSize;
        private PiperSchollerData selectedPoint;

        public List<PiperSchollerData> PiperSchollerPoints { get; private set; }

        public PiperSchollerData SelectedPoint { get => selectedPoint; set { SetValue(ref selectedPoint, value); } }

        public bool ShowZones { get => showZones; set { SetValue(ref showZones, value); _Dialog?.CreateGraphics(); } }


        public int PointSize
        {
            get => pointSize;
            set
            {
                if (value >= 10 && value <= 1000)
                {
                    SetValue(ref pointSize, value);
                    _Dialog?.CreateGraphics();
                }
            }
        }

        public PiperSchoellerGraphicViewModel(IEnumerable<Well> wells) : base(null)
        {
            showZones = true;
            pointSize = 100;
            Initialize();
            InitializeData(wells);
        }

        public PiperSchoellerGraphicViewModel(IEnumerable<WaterAnalysis> analyses) : base(null)
        {
            showZones = true;
            pointSize = 100;
            Initialize();
            InitializeData(analyses);
        }


        void InitializeData(IEnumerable<WaterAnalysis> analyses)
        {
            PiperSchollerPoints = new List<PiperSchollerData>();

            var group = analyses.GroupBy(a => a.Well);

            foreach (var g in group)
            {
                foreach (var a in g)
                {
                    var aColor = SharedBaseView.GetRandomColor(_Random);
                    PiperSchollerPoints.Add(CalculatePercentage(a, aColor));
                }
            }

        }

        void InitializeData(IEnumerable<Well> wells)
        {
            PiperSchollerPoints = new List<PiperSchollerData>();

            foreach (var w in wells)
            {
                var aColor = SharedBaseView.GetRandomColor(_Random);
                foreach (var a in w.WaterAnalyses)
                {
                    PiperSchollerPoints.Add(CalculatePercentage(a, aColor));
                }
            }

        }

        protected override void SetCommandUpdates()
        {
            Add(nameof(SelectedPoint), ChangeColorCommand);
        }

        PiperSchollerData CalculatePercentage(WaterAnalysis analysis, Color color)
        {
            var cal = analysis.Calcium / CalciumMeq;
            var pot = analysis.Potassium / PotassiumMeq;
            var sod = analysis.Sodium / SodiumMeq;
            var mag = analysis.Magnesium / MagnesiumMeq;
            var totalCations = cal + pot + sod + mag;

            var bicarb = analysis.BicarbonateAlkalinity / BicarbonateMeq;
            var sulf = analysis.Sulfates / SulfatesMeq;
            var chlo = analysis.Chlorides / ChloridesMeq;
            var totalAnions = bicarb + sulf + chlo;

            var psData = new PiperSchollerData
            {
                Calcium = cal / totalCations * 100,
                Magnesium = mag / totalCations * 100,
                PotassiumAndSodium = (sod + pot) / totalCations * 100,
                Carbonates = bicarb / totalAnions * 100,
                Chlorides = chlo / totalAnions * 100,
                Sulfates = sulf / totalAnions * 100,
                PointColor = color,
                Label = $"{analysis.WellName} ({analysis.ToString()})",
                IsVisible = true
            };

            return psData;
        }

        protected override void OnSetView(IView view)
        {
            base.OnSetView(view);
            _Dialog = (view as IPiperSchoellerGraphicView);
        }



        public ICommand SaveImageCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    _Dialog.SaveImage("Piper");
                }, (obj) => true, OnError);
            }
        }

        public ICommand ChangeColorCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    var currentColor = SelectedPoint.PointColor.ToDrawingColor();
                    SelectedPoint.PointColor = SharedBaseView.ShowColorDialog(currentColor);
                    _Dialog.CreateGraphics();
                }, (obj) => SelectedPoint != null, OnError);
            }
        }

    }
}
