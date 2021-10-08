using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HotelSystem.DataLayer;
using HotelSystem.Model;
using Microsoft.Win32;

namespace HotelSystem.ViewModel
{
    public class ClientsTabViewModel : ViewModelBase
    {
        private IList<Client> _filteredClientList;

        public ClientRepository ClientRepository { get; }
        public RoomRepository RoomRepository { get;  } 
        public Client ClientInfo { get; set; } = new Client();
        public Client ClientFilter { get; set; } = new Client();
        public Client SelectedClient { get; set; }

        public IList<Client> FilteredClientList
        {
            get => _filteredClientList;
            set
            {
                _filteredClientList = value;
                RaisePropertyChanged();
            }
        }

        public ClientsTabViewModel(ClientRepository clientRepository, RoomRepository roomRepository)
        {
            ClientRepository = clientRepository;
            RefreshClientList();

            RoomRepository = roomRepository;
        }

        private void RefreshClientList()
        {
            RaisePropertyChanged(nameof(Clients));
        }

        public ObservableCollection<Client> Clients {  
            get {
                return new ObservableCollection<Client>(ClientRepository.GetAllClients());
            } 
        }
        public ObservableCollection<Room> Rooms { 
            get {
                return new ObservableCollection<Room>(RoomRepository.GetAllRooms()); 
            } 
        }

        #region Commands

        private RelayCommand _addClientCommand;
        private RelayCommand _updateClientCommand;
        private RelayCommand _deleteClientCommand;
        private RelayCommand _exportClientsCommand;
        private RelayCommand<object> _resetFilterClientCommand;
        private RelayCommand _clientsGridSelectionChangedCommand;
        private RelayCommand _clientsFilterChangedCommand;

        public ICommand AddClientCommand =>
            _addClientCommand ??
            (_addClientCommand = new RelayCommand(
                () =>
                {
                    ClientRepository.StoreClient(new Client
                    {
                        FirstName = ClientInfo.FirstName,
                        LastName = ClientInfo.LastName,
                        Birthdate = ClientInfo.Birthdate,
                        Account = ClientInfo.Account,
                        Room = ClientInfo.Room
                    });
                    RefreshClientList();
                },
                () =>
                {
                    if (string.IsNullOrEmpty(ClientInfo.FirstName)
                        || string.IsNullOrEmpty(ClientInfo.LastName)
                        || ClientInfo.Room == null)
                    {
                        return false;
                    }
                    return true;
                }));

        public ICommand UpdateClientCommand =>
            _updateClientCommand ??
            (_updateClientCommand = new RelayCommand(
                () =>
                {
                    ClientRepository.ChangeClient(SelectedClient?.Id, ClientInfo);
                    RefreshClientList();
                },
                () =>
                {
                    if (SelectedClient == null) return false;
                    if (string.IsNullOrEmpty(ClientInfo.FirstName)
                        || string.IsNullOrEmpty(ClientInfo.LastName)
                        || ClientInfo.Room == null)
                    {
                        return false;
                    }
                    return true;
                }));

        public ICommand DeleteClientCommand =>
            _deleteClientCommand ??
            (_deleteClientCommand = new RelayCommand(
                () =>
                {
                    ClientRepository.RemoveClient(SelectedClient?.Id);
                    RefreshClientList();
                },
                () => SelectedClient != null));

        public ICommand ExportClientsCommand =>
            _exportClientsCommand ??
            (_exportClientsCommand = new RelayCommand(
                () =>
                {
                    var clientsExport = ClientRepository.GetAllClients()
                                                        .Select(client => new ClientExport
                    {
                        FirstName = client.FirstName,
                        LastName = client.LastName,
                        Birthdate = client.Birthdate.Value, // TODO Fix bug if client has no birthday
                        Account = client.Account,
                        RoomNumber = client.Room.Number
                    });
                    /* TODO move to seperate class */
                    var saveDialog = new SaveFileDialog
                    {
                        DefaultExt = ".xls",
                        Filter = "Excel table (.xls)|*.xls"
                    };
                    var result = saveDialog.ShowDialog();
                    if (result == true)
                    {
                        using (TextWriter sw = new StreamWriter(saveDialog.FileName))
                        {
                            var reportCreator = new ReportCreator();
                            reportCreator.WriteTsv(clientsExport, sw);
                            sw.Close();
                        }
                    }
                },
                () => ClientRepository.HasClients()));

        public RelayCommand<object> ResetFilterClientCommand =>
            _resetFilterClientCommand ??
            (_resetFilterClientCommand = new RelayCommand<object>(
                parameters =>
                {
                    if (parameters is Tuple<TextBox, TextBox, DatePicker, TextBox, ComboBox> tuple)
                    {
                        tuple.Item1.Text = string.Empty;
                        tuple.Item2.Text = string.Empty;
                        tuple.Item3.SelectedDate = null;
                        tuple.Item4.Text = string.Empty;
                        tuple.Item5.SelectedIndex = -1;
                    }
                },
                parameters =>
                {
                    if (parameters == null) return false;
                    if (parameters is Tuple<TextBox, TextBox, DatePicker, TextBox, ComboBox> tuple)
                    {
                        if (string.IsNullOrEmpty(tuple.Item1.Text)
                            && string.IsNullOrEmpty(tuple.Item2.Text)
                            && tuple.Item3.SelectedDate == null
                            && string.IsNullOrEmpty(tuple.Item4.Text)
                            && (tuple.Item5 == null || tuple.Item5.SelectedIndex == -1))
                            return false;
                        return true;
                    }
                    return false;
                }));

        public ICommand ClientsGridSelectionChangedCommand =>
            _clientsGridSelectionChangedCommand ??
            (_clientsGridSelectionChangedCommand =
                new RelayCommand(
                    () =>
                    {
                        ClientInfo.FirstName = SelectedClient.FirstName;
                        ClientInfo.LastName = SelectedClient.LastName;
                        ClientInfo.Birthdate = SelectedClient.Birthdate;
                        ClientInfo.Account = SelectedClient.Account;
                        ClientInfo.Room = SelectedClient.Room;
                    },
                    () => SelectedClient != null));

        public ICommand ClientsFilterChangedCommand =>
            _clientsFilterChangedCommand ?? (_clientsFilterChangedCommand =
                new RelayCommand(() =>
                {
                    IEnumerable<Client> queryResult = Clients;
                    if (!string.IsNullOrEmpty(ClientFilter.FirstName))
                    {
                        queryResult = queryResult.Where(client => client.FirstName.Contains(ClientFilter.FirstName));
                    }
                    if (!string.IsNullOrEmpty(ClientFilter.LastName))
                    {
                        queryResult = queryResult.Where(client => client.LastName.Contains(ClientFilter.LastName));
                    }
                    if (ClientFilter.Birthdate != null)
                    {
                        queryResult = queryResult.Where(client => client.Birthdate == ClientFilter.Birthdate);
                    }
                    if (!string.IsNullOrEmpty(ClientFilter.Account))
                    {
                        queryResult = queryResult.Where(client => client.Account.Contains(ClientFilter.Account));
                    }
                    if (ClientFilter.Room != null)
                    {
                        queryResult = queryResult.Where(client => client.Room == ClientFilter.Room);
                    }
                    FilteredClientList = queryResult?.ToList();
                }));

        #endregion
    }
}