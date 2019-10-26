using FluentValidation;
using LaTeAndes.CoreView;
using LaTeAndes.CoreView.Validators;
using LaTeAndes.CoreView.ViewInterfaces;
using LaTeAndes.CoreView.ViewModel;
using LaTeAndes.DataModel.Data.Repositories;
using LaTeAndes.DataModel.Models;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;

namespace LaTeAndes.DesktopCore.ViewModels
{
    public class ContractViewModel : BaseViewModel
    {
        IContractView _Dialog;
        Client _Client;
        Contract _NewContract;
        string _ContractName;
        Billable _SelectedBillable;
        ObservableCollection<ContractedBillable> _Billables;
        List<ContractedStudy> _SelectedStudies;
        ContractedStudy _SelectedStudy;
        ObservableCollection<ContractedStudy> _ContractedStudies;
        string _DialogTitle;
        ObservableCollection<ExternalFile> _Files;
        ExternalFile _SelectedFile;
        readonly bool _IsEditing;

        public bool IsEditing => _IsEditing;
        public Contract NewContract => _NewContract;
        public string DialogTitle { get => _DialogTitle; set { SetValue(ref _DialogTitle, value); } }
        public string ContractName { get => _ContractName; set { SetValue(ref _ContractName, value); } }
        public Billable SelectedBillable { get => _SelectedBillable; set { SetValue(ref _SelectedBillable, value); } }
        public ExternalFile SelectedFile { get => _SelectedFile; set { SetValue(ref _SelectedFile, value); } }
        public ObservableCollection<ContractedBillable> Billables { get => _Billables; set { SetValue(ref _Billables, value); } }
        public ObservableCollection<ContractedStudy> ContractedStudies { get => _ContractedStudies; set { SetValue(ref _ContractedStudies, value); } }
        public ObservableCollection<ExternalFile> Files { get => _Files; set { SetValue(ref _Files, value); } }
        public ContractedStudy SelectedStudy { get => _SelectedStudy; set { SetValue(ref _SelectedStudy, value); } }
        public double TotalAmount
        {
            get
            {
                if (Billables.Any())
                    return Billables.Sum(b => b.OriginalQuantity * b.Price);
                return 0.0;
            }
        }

        public IList SelectedStudies
        {
            get => _SelectedStudies;
            set
            {
                var list = new List<ContractedStudy>();
                foreach (var o in value)
                {
                    if (o.GetType().Name.Contains(nameof(ContractedStudy)))
                        list.Add(o as ContractedStudy);
                }
                SetValue(ref _SelectedStudies, list);
            }
        }


        public ContractViewModel(Client client) : base(null)
        {
            _Client = client;
            _IsEditing = false;
            _NewContract = new Contract();
            _ContractedStudies = new ObservableCollection<ContractedStudy>(RepositoryWrapper.Instance.AvailableStudiesRepository.All.Select(a => a.GetContractedStudy()));
            _ContractedStudies.CollectionChanged += OnContractedStudiesChanged;
            _Files = new ObservableCollection<ExternalFile>();
            _Billables = new ObservableCollection<ContractedBillable>();
            _Billables.CollectionChanged += OnBillablesChanged;
            DialogTitle = "Contrato nuevo";
        }

        public ContractViewModel(Contract contract) : base(null)
        {
            _IsEditing = true;
            _Client = contract.Client;
            _NewContract = contract;
            _ContractName = contract.Name;
            _ContractedStudies = new ObservableCollection<ContractedStudy>(contract.ContractedStudies);
            _Files = new ObservableCollection<ExternalFile>(contract.Files);
            _Billables = new ObservableCollection<ContractedBillable>(contract.Billables);
            _Billables.CollectionChanged += OnBillablesChanged;
            _ContractedStudies.CollectionChanged += OnContractedStudiesChanged;
            DialogTitle = $"Editar {_ContractName}";
        }


        public ICommand NewContractCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    UpdateNewContract();
                    _Dialog.CloseView(true);
                }, (obj) => !HasFailures, OnError);
            }
        }

        public ICommand AddBillableCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    var vm = new BillableViewModel(_SelectedStudies);
                    if (_Dialog.OpenBillableView(vm))
                    {
                        Billables.Add(vm.Billable);
                        SelectedBillable = vm.Billable;
                    }
                }, (obj) => _SelectedStudies != null && _SelectedStudies.Any(), OnError);
            }
        }

        public ICommand EditBillableCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    var vm = new BillableViewModel((ContractedBillable)_SelectedBillable);
                    if (_Dialog.OpenBillableView(vm))
                        OnBillablesChanged(this, null);
                }, (obj) => _SelectedBillable != null, OnError);
            }
        }

        public ICommand RemoveBillableCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    if (_Dialog.ShowMessageBox("¿Desea eliminar el facturable seleccionado?", "Eliminar facturable"))
                    {
                        _Billables.Remove((ContractedBillable)SelectedBillable);
                        SelectedBillable = null;
                    }
                }, (obj) => _SelectedBillable != null, OnError);
            }
        }

        public ICommand RemoveStudyCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    if (_Dialog.ShowMessageBox("¿Desea eliminar el estudio seleccionado?", "Eliminar estudio"))
                    {
                        _ContractedStudies.Remove(SelectedStudy);
                        SelectedStudy = null;
                    }
                }, (obj) => SelectedStudy != null, OnError);
            }
        }

        public ICommand AddFileCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    var filename = _Dialog.OpenFileDialog("Cualquier archivo|*.*", "Cargar archivo");
                    if (!string.IsNullOrEmpty(filename))
                    {
                        if (!_Files.ToList().Exists(f => f.CompleteFilename == System.IO.Path.GetFileName(filename)))
                        {
                            var file = new ExternalFile(filename);
                            _Files.Add(file);
                            SelectedFile = file;
                        }
                        else
                        {
                            _Dialog.ShowErrorMessageBox("Ya está cargado un archivo con el mismo nombre");
                        }
                    }
                }, (obj) => true, OnError);
            }
        }

        public ICommand ViewFileCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    SelectedFile.Open();
                }, (obj) => SelectedFile != null, OnError);
            }
        }

        public ICommand RemoveFileCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    if (_Dialog.ShowMessageBox("¿Desea eliminar el archivo seleccionado?", "Eliminar archivo"))
                    {
                        _Files.Remove(SelectedFile);
                        RepositoryWrapper.Instance.ExternalFilesRepository.Remove(SelectedFile);
                        SelectedFile = null;
                    }
                }, (obj) => SelectedFile != null, OnError);
            }
        }


        protected override void OnSetView(IView view)
        {
            base.OnSetView(view);
            Initialize();
            _Dialog = (IContractView)view;
            Validate(nameof(ContractedStudies), ContractedStudies);
            if (_IsEditing)
                ValidateAll();
        }

        protected override void SetCommandUpdates()
        {
            Add(nameof(HasFailures), NewContractCommand);
            Add(nameof(SelectedBillable), new List<ICommand>() { RemoveBillableCommand, EditBillableCommand });
            Add(nameof(SelectedFile), new List<ICommand>() { ViewFileCommand, RemoveFileCommand });
            Add(nameof(SelectedStudy), RemoveStudyCommand);
            Add(nameof(SelectedStudies), AddBillableCommand);
        }

        protected override void SetValidators()
        {
            Add(nameof(ContractName), new EmptyStringValidator("Nombre"));
            Add(nameof(Billables), new List<IValidator>() { new EmptyListValidator<ContractedBillable>("Facturables"),
                new ListFunctionValidator<ContractedBillable>("Precio de los facturables", b => b.Price > 0.0) });
            Add(nameof(TotalAmount), new NumberValidator<double>("Facturables", NumericFunctions.Greater, 0.0));
            Add(nameof(ContractedStudies), new List<IValidator>() { new EmptyListValidator<ContractedStudy>("Estudios"),
                new ListFunctionValidator<ContractedStudy>("Precio de los estudios", c => c.Price > 0.0),
                new ListFunctionValidator<ContractedStudy>("Factor de equivalencia de los estudios", c => c.EquivalencyFactor > 0.0) });

        }

        public void OnBillablesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Validate(nameof(Billables), Billables);
            NotifyPropertyChanged(nameof(TotalAmount));
            Validate(nameof(TotalAmount), TotalAmount);
            UpdateCommandsThatDependsOnFailures();
        }

        public void OnContractedStudiesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Validate(nameof(ContractedStudies), ContractedStudies);
            NotifyPropertyChanged(nameof(TotalAmount));
            UpdateCommandsThatDependsOnFailures();
        }

        private void UpdateNewContract()
        {
            _NewContract.Name = ContractName;
            _NewContract.Billables = Billables.ToList();
            _NewContract.ContractedStudies = ContractedStudies.ToList();
            _NewContract.Files = Files.ToList();
            _NewContract.TotalAmount = TotalAmount;
            _NewContract.Client = _Client;

            foreach (var b in _NewContract.Billables)
            {
                b.Contract = _NewContract;
                //b.Client = _Client;
            }

            foreach (var cs in _NewContract.ContractedStudies)
            {
                cs.Contract = _NewContract;
            }
        }
    }
}
