using FluentValidation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Wells.Base;
using Wells.Persistence.Repositories;
using Wells.BaseView;
using Wells.BaseView.Validators;
using Wells.BaseView.ViewInterfaces;
using Wells.BaseView.ViewModel;
using Wells.BaseModel.Models;
using Wells.Model;

namespace Wells.View.ViewModels
{
    public class EditWellViewModel : BaseViewModel
    {
        private string wellName;
        private double z;
        private double latitude;
        private double longitude;
        private int type;
        private double height;
        private bool exists;
        private double bottom;
        private IEditWellView dialog;
        private ObservableCollection<WaterAnalysis> waterAnalyses;
        private ObservableCollection<Measurement> measurements;
        private ObservableCollection<ExternalFile> files;

        public string WellName { get => wellName; set { SetValue(ref wellName, value); } }
        public double Z { get => z; set { SetValue(ref z, value); } }
        public double Latitude { get => latitude; set { SetValue(ref latitude, value); } }
        public double Longitude { get => longitude; set { SetValue(ref longitude, value); } }
        public int Type { get => type; set { SetValue(ref type, value); } }
        public double Height { get => height; set { SetValue(ref height, value); } }
        public bool Exists { get => exists; set { SetValue(ref exists, value); } }
        public double Bottom { get => bottom; set { SetValue(ref bottom, value); } }
        public ObservableCollection<WaterAnalysis> WaterAnalyses { get => waterAnalyses; set { SetValue(ref waterAnalyses, value); } }
        public ObservableCollection<Measurement> Measurements { get => measurements; set { SetValue(ref measurements, value); } }
        public ObservableCollection<ExternalFile> Files { get => files; set { SetValue(ref files, value); } }

        public bool IsEditing { get; }


        public Well Well { get; }


        public EditWellViewModel() : base(null)
        {
            IsEditing = false;
            Well = new Well();
            InitializeWell();
        }

        public EditWellViewModel(Well well) : base(null)
        {
            IsEditing = true;
            this.Well = well;
            InitializeWell();
        }

        protected override void OnSetView(IView view)
        {
            base.OnSetView(view);
            dialog = (IEditWellView)view;
            Initialize();
        }

        void InitializeWell()
        {
            WellName = Well.Name;
            Z = Well.Z;
            Latitude = Well.Latitude;
            Longitude = Well.Longitude;
            Height = Well.Height;
            Exists = Well.Exists;
            Bottom = Well.Bottom;
            WaterAnalyses = new ObservableCollection<WaterAnalysis>(Well.WaterAnalyses.OrderBy(i => i.Date));
            Measurements = new ObservableCollection<Measurement>(Well.Measurements.OrderBy(i => i.Date));
            Files = new ObservableCollection<ExternalFile>(Well.Files.OrderBy(i => i.CompleteFilename));
        }


        void SaveWell()
        {
            Well.Name = WellName;
            Well.Z = Z;
            Well.Latitude = Latitude;
            Well.Longitude = Longitude;
            Well.Bottom = Bottom;
            Well.Exists = Exists;
            Well.Height = Height;
            Well.WaterAnalyses = WaterAnalyses.ToList();
            Well.Measurements = Measurements.ToList();
            Well.Files = Files.ToList();
        }

        protected override void SetCommandUpdates() 
        {
            Add(nameof(HasFailures), SaveWellCommand);
        }

        protected override void SetValidators()
        {
            if (!IsEditing)
            {
                Add(nameof(WellName), new List<IValidator> { new EmptyStringValidator("Nombre"), new WellNameValidator("Nombre") });
            }
        }

        void RemoveAll()
        {
            RepositoryWrapper.Instance.Measurements.RemoveRange(Well.Measurements);
            RepositoryWrapper.Instance.WaterAnalyses.RemoveRange(Well.WaterAnalyses);
            RepositoryWrapper.Instance.ExternalFiles.RemoveRange(Well.Files);
            RepositoryWrapper.Instance.Wells.Remove(Well);
        }


        public ICommand DeleteWellCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    if (SharedBaseView.ShowYesNoMessageBox(View, "¿Está seguro de eliminar este pozo? Esta operación no se puede deshacer.", "Eliminar"))
                    {
                        RemoveAll();
                        RepositoryWrapper.Instance.SaveChanges();
                        CloseModalViewCommand.Execute(true);
                    }
                }, (obj) => IsEditing, OnError);
            }
        }

        public ICommand SaveWellCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    SaveWell();
                    if (IsEditing)
                    {
                        RepositoryWrapper.Instance.Wells.Update(Well);
                    }
                    else
                    {
                        RepositoryWrapper.Instance.Wells.Add(Well);
                    }
                    RepositoryWrapper.Instance.SaveChanges();
                    CloseModalViewCommand.Execute(true);
                }, (obj) => !HasFailures, OnError);
            }
        }

        public ICommand NewMeasurementCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    var vm = new EditMeasurementViewModel(Well);
                    if (dialog.ShowEditMeasurementDialog(vm))
                    {
                        Measurements.Add(vm.Measurement);
                    }
                }, (obj) => true, OnError);
            }
        }

        public ICommand EditMeasurementCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    var vm = new EditMeasurementViewModel(param as Measurement);
                    if (dialog.ShowEditMeasurementDialog(vm))
                    {
                        Measurements.Add(vm.Measurement);
                    }
                }, (obj) => true, OnError);
            }
        }

        public ICommand DeleteMeasurementCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    if(param is Measurement measurement)
                    {
                        if (SharedBaseView.ShowYesNoMessageBox(View, "¿Desea borrar esta medición? Esta operación no se puede deshacer.", "Borrar medición"))
                        {
                            Measurements.Remove(measurement);
                            RepositoryWrapper.Instance.Measurements.Remove(measurement);
                        }
                    }
                }, (obj) => true, OnError);
            }
        }


        public ICommand DeleteWaterAnalysisCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    if (param is WaterAnalysis analysis)
                    {
                        if (SharedBaseView.ShowYesNoMessageBox(View, "¿Desea borrar este análisis? Esta operación no se puede deshacer.", "Borrar análisis"))
                        {
                            WaterAnalyses.Remove(analysis);
                            RepositoryWrapper.Instance.WaterAnalyses.Remove(analysis);
                        }
                    }
                }, (obj) => true, OnError);
            }
        }


        public ICommand NewExternalLinkCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    var filename = SharedBaseView.OpenFileDialog("Archivos|*.*", "Elija un archivo");

                    if (!string.IsNullOrEmpty(filename))
                    {
                        var file = new ExternalFile(filename) { Well = Well };
                        Files.Add(file);
                    }
                }, (obj) => true, OnError);
            }
        }

        public ICommand OpenExternalLinkCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    if (param != null)
                    {
                        var file = param as ExternalFile;
                        file.Open();
                    }
                }, (obj) => true, OnError);
            }
        }

        public ICommand DeleteExternalLinkCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    if (param != null)
                    {
                        var file = param as ExternalFile;
                        if (SharedBaseView.ShowYesNoMessageBox(View, "¿Desea borrar este archivo? Esta operación no se puede deshacer.", "Borrar archivo"))
                        {
                            Files.Remove(file);
                            RepositoryWrapper.Instance.ExternalFiles.Remove(file);
                        }
                    }
                }, (obj) => true, OnError);
            }
        }
    }
}