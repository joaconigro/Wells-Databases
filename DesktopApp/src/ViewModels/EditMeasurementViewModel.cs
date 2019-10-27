using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Wells.CorePersistence.Repositories;
using Wells.CoreView;
using Wells.CoreView.Validators;
using Wells.CoreView.ViewModel;
using Wells.YPFModel;

namespace Wells.View.ViewModels
{
    public class EditMeasurementViewModel : BaseViewModel
    {
        private Well selectedWell;
        private DateTime date;
        private double fLNADepth;
        private double waterDepth;
        private string comment;


        public string WellName { get => selectedWell?.Name; }
        public List<Well> Wells => RepositoryWrapper.Instance.Wells.All.ToList();
        public Well SelectedWell { get => selectedWell; set { SetValue(ref selectedWell, value); NotifyPropertyChanged(nameof(WellName)); } }
        public Measurement Measurement { get; }

        public DateTime Date { get => date; set { SetValue(ref date, value); } }
        public double FLNADepth { get => fLNADepth; set { SetValue(ref fLNADepth, value); } }
        public double WaterDepth { get => waterDepth; set { SetValue(ref waterDepth, value); } }
        public string Comment { get => comment; set { SetValue(ref comment, value); } }
        public bool IsEditing { get; }
        public bool IsWellSelectable { get; }


        public EditMeasurementViewModel() : base(null)
        {
            IsEditing = false;
            IsWellSelectable = true;
            Measurement = new Measurement();
            InitializeMeasurement();
        }

        public EditMeasurementViewModel(Well well) : base(null)
        {
            IsEditing = false;
            IsWellSelectable = false;
            Measurement = new Measurement() { Well = well };
            InitializeMeasurement();
        }

        public EditMeasurementViewModel(Measurement measurement) : base(null)
        {
            IsEditing = true;
            IsWellSelectable = false;
            Measurement = measurement;
            InitializeMeasurement();
        }

        void InitializeMeasurement()
        {
            SelectedWell = Measurement.Well;
            Date = Measurement.Date;
            FLNADepth = Measurement.FLNADepth;
            WaterDepth = Measurement.WaterDepth;
            Comment = Measurement.Comment;
        }

        void SaveMeasurement()
        {
            Measurement.Well = SelectedWell;
            Measurement.Date = Date;
            Measurement.FLNADepth = FLNADepth;
            Measurement.WaterDepth = WaterDepth;
            Measurement.Comment = Comment;
        }

        protected override void SetCommandUpdates() { }

        protected override void SetValidators()
        {
            Add(nameof(WellName), new EmptyStringValidator("Nombre"));
        }

        public ICommand DeleteMeasurementCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    if (View.ShowYesNoMessageBox("¿Está seguro de eliminar esta medición?", "Eliminar"))
                    {
                        RepositoryWrapper.Instance.Measurements.Remove(Measurement);
                        RepositoryWrapper.Instance.SaveChanges();
                        CloseModalViewCommand.Execute(true);
                    }
                }, (obj) => !IsEditing, OnError);
            }
        }

        public ICommand SaveMeasurementCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    SaveMeasurement();
                    if (IsEditing)
                        RepositoryWrapper.Instance.Measurements.Update(Measurement);
                    else
                        RepositoryWrapper.Instance.Measurements.Add(Measurement);
                    RepositoryWrapper.Instance.SaveChanges();
                    CloseModalViewCommand.Execute(true);
                }, (obj) => !HasFailures, OnError);
            }
        }

    }

}
