using FluentValidation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Wells.Base;
using Wells.CorePersistence.Repositories;
using Wells.CoreView;
using Wells.CoreView.Validators;
using Wells.CoreView.ViewInterfaces;
using Wells.CoreView.ViewModel;
using Wells.StandardModel.Models;
using Wells.YPFModel;

namespace Wells.View.ViewModels
{
    public class EditWellViewModel : BaseViewModel
    {
        private Well well;
        private string wellName;
        private double x;
        private double y;
        private double z;
        private double latitude;
        private double longitude;
        private int type;
        private double height;
        private bool exists;
        private double bottom;
        private IEditWellView dialog;
        public string WellName { get => wellName; set { SetValue(ref wellName, value); } }


        public double X { get => x; set { SetValue(ref x, value); } }
        public double Y { get => y; set { SetValue(ref y, value); } }
        public double Z { get => z; set { SetValue(ref z, value); } }
        public double Latitude { get => latitude; set { SetValue(ref latitude, value); } }
        public double Longitude { get => longitude; set { SetValue(ref longitude, value); } }
        public int Type { get => type; set { SetValue(ref type, value); } }
        public double Height { get => height; set { SetValue(ref height, value); } }
        public bool Exists { get => exists; set { SetValue(ref exists, value); } }
        public double Bottom { get => bottom; set { SetValue(ref bottom, value); } }
        public ObservableCollection<SoilAnalysis> SoilAnalyses { get; set; }
        public ObservableCollection<WaterAnalysis> WaterAnalyses { get; set; }
        public ObservableCollection<FLNAAnalysis> FLNAAnalyses { get; set; }
        public ObservableCollection<Measurement> Measurements { get; set; }
        public ObservableCollection<ExternalFile> Files { get; set; }

        public bool IsEditing { get; }

        public List<string> WellTypes => Common.EnumDescriptionsToList(typeof(WellType));


        public Well Well => well;


        public EditWellViewModel() : base(null)
        {
            IsEditing = false;
            well = new Well();
            InitializeWell();
        }

        public EditWellViewModel(Well well) : base(null)
        {
            IsEditing = true;
            this.well = well;
            InitializeWell();
    }

        protected override void OnSetView(IView view)
        {
            base.OnSetView(view);
            dialog = (IEditWellView)view;
        }

        void InitializeWell()
        {
            WellName = well.Name;
            X = well.X;
            Y = well.Y;
            Z = well.Z;
            Latitude = well.Latitude;
            Longitude = well.Longitude;
            Type = (int)well.WellType;
            Height = well.Height;
            Exists = well.Exists;
            Bottom = well.Bottom;
            SoilAnalyses = new ObservableCollection<SoilAnalysis>(well.SoilAnalyses.OrderBy(i => i.Date));
            WaterAnalyses = new ObservableCollection<WaterAnalysis>(well.WaterAnalyses.OrderBy(i => i.Date));
            FLNAAnalyses = new ObservableCollection<FLNAAnalysis>(well.FLNAAnalyses.OrderBy(i => i.Date));
            Measurements = new ObservableCollection<Measurement>(well.Measurements.OrderBy(i => i.Date));
            Files = new ObservableCollection<ExternalFile>(well.Files.OrderBy(i => i.CompleteFilename));
        }


        void SaveWell()
        {
            well.Name = WellName;
            well.X = X;
            well.Y = Y;
            well.Z = Z;
            well.Latitude = Latitude;
            well.Longitude = Longitude;
            well.WellType = (WellType)Type;
            well.Bottom = Bottom;
            well.Exists = Exists;
            well.Height = Height;
            well.SoilAnalyses = SoilAnalyses.ToList();
            well.WaterAnalyses = WaterAnalyses.ToList();
            well.FLNAAnalyses = FLNAAnalyses.ToList();
            well.Measurements = Measurements.ToList();
            well.Files = Files.ToList();
        }

        protected override void SetCommandUpdates()
        {
            throw new NotImplementedException();
        }

        protected override void SetValidators()
        {
            Add(nameof(WellName), new List<IValidator>() { new EmptyStringValidator("Nombre"), new WellNameValidator("Nombre") }) ;
        }

        void RemoveAll()
        {
            RepositoryWrapper.Instance.Measurements.RemoveRange(well.Measurements);
            RepositoryWrapper.Instance.SoilAnalyses.RemoveRange(well.SoilAnalyses);
            RepositoryWrapper.Instance.WaterAnalyses.RemoveRange(well.WaterAnalyses);
            RepositoryWrapper.Instance.FLNAAnalyses.RemoveRange(well.FLNAAnalyses);
            RepositoryWrapper.Instance.ExternalFiles.RemoveRange(well.Files);
            RepositoryWrapper.Instance.Wells.Remove(well);
        }


        public ICommand DeleteWellCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    if (View.ShowYesNoMessageBox("¿Está seguro de eliminar este pozo?", "Eliminar"))
                    {
                        RemoveAll();
                        RepositoryWrapper.Instance.SaveChanges();
                        CloseModalViewCommand.Execute(true);
                    }
                }, (obj) => !IsEditing, OnError);
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
                        RepositoryWrapper.Instance.Wells.Update(well);
                    else
                        RepositoryWrapper.Instance.Wells.Add(well);
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
                    var vm = new EditMeasurementViewModel(well);
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
                    var measurement = (param as Measurement);
                    if (dialog.ShowYesNoMessageBox("¿Desea borrar esta medición?", "Borrar medición"))
                    {
                        Measurements.Remove(measurement);
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
                    var filename = View.OpenFileDialog("Archivos|*.*", "Elija un archivo");

                    if (!string.IsNullOrEmpty(filename))
                    {
                        var file = new ExternalFile(filename) { Well = well };
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
                    if(param != null)
                    {
                        var file = (param as ExternalFile);
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
                        var file = (param as ExternalFile);
                        if (dialog.ShowYesNoMessageBox("¿Desea borrar este archivo?", "Borrar archivo"))
                        {
                            Files.Remove(file);
                        }
                    }
                }, (obj) => true, OnError);
            }
        }
    }
}