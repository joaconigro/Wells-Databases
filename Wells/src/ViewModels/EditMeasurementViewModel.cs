using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Wells.BaseView;
using Wells.BaseView.Validators;
using Wells.BaseView.ViewInterfaces;
using Wells.BaseView.ViewModel;
using Wells.Model;
using Wells.Persistence.Repositories;

namespace Wells.View.ViewModels
{
    public class EditMeasurementViewModel : BaseViewModel
    {
        private Well selectedWell;
        private DateTime date;
        private double flnaDepth;
        private double waterDepth;
        private string comment;


        public string WellName { get => selectedWell?.Name; }
        public List<Well> Wells => RepositoryWrapper.Instance.Wells.All.ToList();
        public Well SelectedWell { get => selectedWell; set { SetValue(ref selectedWell, value); Validate(nameof(WellName), WellName); } }
        public Measurement Measurement { get; }

        public DateTime Date { get => date; set { SetValue(ref date, value); } }
        public double FlnaDepth { get => flnaDepth; set { SetValue(ref flnaDepth, value); } }
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
            Measurement = new Measurement { Well = well };
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
            selectedWell = Measurement.Well;
            Date = Measurement.Date;
            FlnaDepth = Measurement.FlnaDepth;
            WaterDepth = Measurement.WaterDepth;
            Comment = Measurement.Comment;
        }

        void SaveMeasurement()
        {
            Measurement.Well = SelectedWell;
            Measurement.Date = Date;
            Measurement.FlnaDepth = FlnaDepth;
            Measurement.WaterDepth = WaterDepth;
            Measurement.Comment = Comment;
        }

        protected override void SetCommandUpdates()
        {
            //No need to implement yet.
        }

        protected override void SetValidators()
        {
            if (!IsEditing)
            {
                Add(nameof(WellName), new EmptyStringValidator("Nombre"));
            }
        }

        protected override void OnSetView(IView view)
        {
            base.OnSetView(view);
            Initialize();
        }

        public ICommand DeleteMeasurementCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    if (SharedBaseView.ShowYesNoMessageBox(View, "¿Está seguro de eliminar esta medición?", "Eliminar"))
                    {
                        RepositoryWrapper.Instance.Measurements.Remove(Measurement);
                        RepositoryWrapper.Instance.SaveChanges();
                        CloseModalViewCommand.Execute(true);
                    }
                }, (obj) => IsEditing, OnError);
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
                    {
                        RepositoryWrapper.Instance.Measurements.Update(Measurement);
                    }
                    else
                    {
                        RepositoryWrapper.Instance.Measurements.Add(Measurement);
                    }
                    RepositoryWrapper.Instance.SaveChanges();
                    CloseModalViewCommand.Execute(true);
                }, (obj) => !HasFailures, OnError);
            }
        }

    }

}
