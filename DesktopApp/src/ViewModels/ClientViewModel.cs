using LaTeAndes.Base;
using LaTeAndes.CoreView;
using LaTeAndes.CoreView.Validators;
using LaTeAndes.CoreView.ViewInterfaces;
using LaTeAndes.CoreView.ViewModel;
using LaTeAndes.DataModel.Data.Repositories;
using LaTeAndes.DataModel.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;

namespace LaTeAndes.DesktopCore.ViewModels
{
    public class ClientViewModel : BaseViewModel
    {
        IClientView _Dialog;
        string _ClientName;
        int _ClientType;
        string _Street;
        string _Number;
        string _Floor;
        string _Department;
        string _City;
        string _ZipCode;
        string _State;
        string _Country;
        string _BillingId;
        int _IVACondition;
        readonly bool _IsEditing;
        Client _Client;
        ClientUser _SelectedClientUser;
        Contract _SelectedContract;
        ObservableCollection<ClientUsersGroup> _Groups;
        ObservableCollection<Contract> _Contracts;
        ObservableCollection<ClientUser> _ClientUsers;
        string _DialogTitle;
        private ClientUsersGroup selectedGroup;

        public bool IsEditing => _IsEditing;
        public string DialogTitle { get => _DialogTitle; set { SetValue(ref _DialogTitle, value); } }
        public string ClientName { get => _ClientName; set { SetValue(ref _ClientName, value?.Trim()); } }
        public int ClientType { get => _ClientType; set { SetValue(ref _ClientType, value); } }
        public string Street { get => _Street; set { SetValue(ref _Street, value?.Trim()); } }
        public string Number { get => _Number; set { SetValue(ref _Number, value?.Trim()); } }
        public string Floor { get => _Floor; set { SetValue(ref _Floor, value?.Trim()); } }
        public string Department { get => _Department; set { SetValue(ref _Department, value?.Trim()); } }
        public string City { get => _City; set { SetValue(ref _City, value?.Trim()); } }
        public string ZipCode { get => _ZipCode; set { SetValue(ref _ZipCode, value?.Trim()); } }
        public string State { get => _State; set { SetValue(ref _State, value?.Trim()); } }
        public string Country { get => _Country; set { SetValue(ref _Country, value?.Trim()); } }
        public string BillingId { get => _BillingId; set { SetValue(ref _BillingId, value?.Trim()); } }
        public int IVACondition { get => _IVACondition; set { SetValue(ref _IVACondition, value); } }
        public List<string> ClientTypes => Common.EnumDescriptionsToList(typeof(ClientTypes));
        public List<string> IVAConditions => Common.EnumDescriptionsToList(typeof(IVAConditions));
        public ClientUser SelectedClientUser { get => _SelectedClientUser; set { SetValue(ref _SelectedClientUser, value); } }
        public Contract SelectedContract { get => _SelectedContract; set { SetValue(ref _SelectedContract, value); } }
        public ClientUsersGroup SelectedGroup { get => selectedGroup; set { SetValue(ref selectedGroup, value); } }
        public List<AvailableStudy> AvalibleStudiesDependency => RepositoryWrapper.Instance.AvailableStudiesRepository.All.ToList();
        public ObservableCollection<ClientUsersGroup> Groups { get => _Groups; set { SetValue(ref _Groups, value); } }
        public ObservableCollection<Contract> Contracts { get => _Contracts; set { SetValue(ref _Contracts, value); } }
        public ObservableCollection<ClientUser> ClientUsers { get => _ClientUsers; set { SetValue(ref _ClientUsers, value); } }


        public ClientViewModel() : base(null)
        {
            _IsEditing = false;
            SetClient(new Client(string.Empty) { Address = new AddressInfo() });
            DialogTitle = "Cliente nuevo";
        }

        public ClientViewModel(Client client) : base(null)
        {
            _IsEditing = true;
            SetClient(client);
            DialogTitle = $"Editar {_ClientName}";
        }

        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    UpdateNewClient();
                    if (_IsEditing)
                        RepositoryWrapper.Instance.ClientsRepository.Update(_Client);
                    else
                        RepositoryWrapper.Instance.ClientsRepository.Add(_Client);
                    RepositoryWrapper.Instance.Save();
                    _Dialog.CloseView(true);
                }, (obj) => !HasFailures, OnError);
            }
        }

        public ICommand NewContractCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    var vm = new ContractViewModel(_Client);
                    if (_Dialog.OpenContractDialog(vm))
                    {
                        _Contracts.Add(vm.NewContract);
                        NotifyPropertyChanged(nameof(Contracts));
                    }
                }, (obj) => true, OnError);
            }
        }

        public ICommand EditContractCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    var vm = new ContractViewModel(SelectedContract);
                    if (_Dialog.OpenContractDialog(vm))
                    {
                        NotifyPropertyChanged(nameof(Contracts));
                    }
                }, (obj) => SelectedContract != null, OnError);
            }
        }

        public ICommand DeleteContractCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    if (_Dialog.ShowMessageBox("¿Está seguro de eliminar este contrato?", "Eliminar contrato"))
                    {
                        _Contracts.Remove(SelectedContract);
                        NotifyPropertyChanged(nameof(Contracts));
                        SelectedContract = null;
                    }
                }, (obj) => SelectedContract != null, OnError);
            }
        }


        public ICommand NewGroupCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    var vm = new ClientUsersGroupViewModel(_Client);
                    if (_Dialog.OpenClientUsersGroupDialog(vm))
                    {
                        _Groups.Add(vm.Group);
                        NotifyPropertyChanged(nameof(Groups));
                    }
                }, (obj) => Contracts.Any(), OnError);
            }
        }

        public ICommand EditGroupCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    var vm = new ClientUsersGroupViewModel(SelectedGroup);
                    if (_Dialog.OpenClientUsersGroupDialog(vm))
                    {
                        NotifyPropertyChanged(nameof(Groups));
                    }
                }, (obj) => SelectedGroup != null && Contracts.Any(), OnError);
            }
        }

        public ICommand DeleteGroupCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    if (_Dialog.ShowMessageBox("¿Está seguro de eliminar este grupo de usuarios?", "Eliminar grupo"))
                    {
                        _Groups.Remove(SelectedGroup);
                        NotifyPropertyChanged(nameof(Groups));
                        SelectedGroup = null;
                    }
                }, (obj) => SelectedGroup != null, OnError);
            }
        }


        public ICommand NewClientUserCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    var vm = new NewClientUserViewModel(_Client);
                    if (_Dialog.OpenNewClientUserDialog(vm))
                    {
                        ClientUsers.Add(vm.NewClientUser);
                        SelectedClientUser = vm.NewClientUser;
                        NotifyPropertyChanged(nameof(ClientUsers));
                        NotifyPropertyChanged(nameof(SelectedClientUser));
                    }
                }, (obj) => true, OnError);
            }
        }

        void SetClient(Client client)
        {
            _Client = client;
            Initialize();
        }

        protected override void Initialize()
        {
            base.Initialize();
            ClientName = _Client.Name;
            ClientType = (int)_Client.ClientType;
            Street = _Client.Address.Street;
            Number = _Client.Address.Number;
            Floor = _Client.Address.Floor;
            Department = _Client.Address.Department;
            City = _Client.Address.City;
            State = _Client.Address.State;
            Country = _Client.Address.Country;
            ZipCode = _Client.Address.ZipCode;
            BillingId = _Client.BillingId;
            Contracts = new ObservableCollection<Contract>(_Client.Contracts);
            ClientUsers = new ObservableCollection<ClientUser>(_Client.ClientUsers);
            Groups = new ObservableCollection<ClientUsersGroup>(_Client.ClientUsersGroups);
            _SelectedClientUser = _Client.RepresentantUser;
            NotifyPropertyChanged(nameof(Contracts));
            NotifyPropertyChanged(nameof(ClientUsers));
            NotifyPropertyChanged(nameof(SelectedClientUser));
            NotifyPropertyChanged(nameof(Groups));
            IVACondition = (int)_Client.IVACondition;

            _Contracts.CollectionChanged += OnContractsChanged;

            if (_IsEditing)
                ValidateAll();
        }

        protected override void OnSetView(IView view)
        {
            base.OnSetView(view);
            _Dialog = (IClientView)view;
        }

        protected override void SetCommandUpdates()
        {
            Add(nameof(HasFailures), SaveCommand);
            Add(nameof(SelectedContract), new List<ICommand>() { EditContractCommand, DeleteContractCommand, NewGroupCommand, EditGroupCommand });
        }

        protected override void SetValidators()
        {
            Add(nameof(ClientName), new EmptyStringValidator("Nombre"));
            Add(nameof(Street), new EmptyStringValidator("Calle"));
            Add(nameof(Number), new EmptyStringValidator("Número"));
            Add(nameof(City), new EmptyStringValidator("Ciudad"));
            Add(nameof(ZipCode), new EmptyStringValidator("Código postal"));
            Add(nameof(State), new EmptyStringValidator("Provincia"));
            Add(nameof(Country), new EmptyStringValidator("País"));
            Add(nameof(BillingId), new EmptyStringValidator("DNI/CUIT/CUIL"));
            Add(nameof(Contracts), new EmptyListValidator<Contract>("Contratos"));
            Add(nameof(SelectedClientUser), new NullObjectValidator("Cliente responsable"));
        }

        public void OnContractsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Validate(nameof(Contracts), Contracts);
            UpdateCommandsThatDependsOnFailures();
        }

        void UpdateNewClient()
        {
            _Client.Name = ClientName;
            _Client.ClientType = (ClientTypes)ClientType;
            _Client.Address.Street = Street;
            _Client.Address.Number = Number;
            _Client.Address.Floor = Floor;
            _Client.Address.Department = Department;
            _Client.Address.City = City;
            _Client.Address.State = State;
            _Client.Address.Country = Country;
            _Client.Address.ZipCode = ZipCode;
            _Client.BillingId = BillingId;
            _Client.IVACondition = (IVAConditions)IVACondition;

            _Client.Contracts = Contracts.ToList();
            _Client.ClientUsers = ClientUsers.ToList();
            _Client.ClientUsersGroups = Groups.ToList();
            _Client.SetRepresentant(_SelectedClientUser);
        }
    }
}
