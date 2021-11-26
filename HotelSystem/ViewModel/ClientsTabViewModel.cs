using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HotelSystem.DataLayer;
using HotelSystem.DataLayer.Models;
using HotelSystem.Model;
using HotelSystem.View;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace HotelSystem.ViewModel
{
    public class ClientsTabViewModel : ViewModelBase
    {
        private IList<Client> _filteredClientList;

        private Client _clientInfo = new Client();

        public IClientRepository ClientRepository { get; }
        public IRoomRepository RoomRepository { get; }
        private IStandardDialog StandardDialog { get; }

        public Client ClientInfo 
        { 
            get => _clientInfo; 
            set
            {
                _clientInfo = value;
                (AddClientCommand as RelayCommand).RaiseCanExecuteChanged(); 
                
            } 
        }
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


        public ClientsTabViewModel(IClientRepository clientRepository, IRoomRepository roomRepository, IStandardDialog standardDialog)
        {
            ClientRepository = clientRepository;
            RefreshClientList();

            RoomRepository = roomRepository;
            StandardDialog = standardDialog;
        }

        private void RefreshClientList()
        {
            RaisePropertyChanged(nameof(Clients));
        }

        public ObservableCollection<Client> Clients
        {
            get
            {
                return new ObservableCollection<Client>(ClientRepository.GetAllClients());
            }
        }
        public ObservableCollection<Room> Rooms
        {
            get
            {
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
                        //Account = ClientInfo.Account,
                        Type = ClientInfo.Type,
                        Room = ClientInfo.Room
                    }) ;
                    RefreshClientList();
                },
                () =>
                {
                    if (string.IsNullOrEmpty(ClientInfo.FirstName)
                        || string.IsNullOrEmpty(ClientInfo.LastName)
                        || ClientInfo.Type == ClientTypes.None
                        || ClientInfo.Room == null)
                    {
                        return false;
                    }
                    if (Clients.Any(client => client.Equals(ClientInfo)))
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
                    if (SelectedClient == null)
                    {
                        return false;
                    }

                    if (string.IsNullOrEmpty(ClientInfo.FirstName)
                        || string.IsNullOrEmpty(ClientInfo.LastName)
                        || ClientInfo.Type == ClientTypes.None
                        || ClientInfo.Room == null)
                    {
                        return false;
                    }

                    if (SelectedClient.Equals(ClientInfo))
                    {
                        return false;
                    }

                    if (!Clients.Any(client => client.Id == SelectedClient.Id))
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
                                                           Birthdate = client.Birthdate,
                                                           // Account = client.Account,
                                                           Type = client.Type,
                                                           RoomNumber = client.Room.Number
                                                       });
                    /* TODO move to seperate class */
                   string exportLocation = StandardDialog.GetExportFilename();
                    /* TODO move to seperate class */
                   if (exportLocation != null)
                   {
                       using (TextWriter sw = new StreamWriter(exportLocation))
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
                    if (parameters is Tuple<TextBox, TextBox, DatePicker, ComboBox, ComboBox> tuple)
                    {
                        tuple.Item1.Text = string.Empty;
                        tuple.Item2.Text = string.Empty;
                        tuple.Item3.SelectedDate = null;
                        //tuple.Item4.Text = string.Empty;
                        tuple.Item4.SelectedIndex = -1;
                        tuple.Item5.SelectedIndex = -1;
                    }
                },
                parameters =>
                {
                    if (parameters == null) return false;
                    if (parameters is Tuple<TextBox, TextBox, DatePicker, ComboBox, ComboBox> tuple)
                    {
                        if (string.IsNullOrEmpty(tuple.Item1.Text)
                            && string.IsNullOrEmpty(tuple.Item2.Text)
                            && tuple.Item3.SelectedDate == null
                            //&& string.IsNullOrEmpty(tuple.Item4.Text)
                            && (tuple.Item4 == null || tuple.Item4.SelectedIndex == -1)
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
                        //ClientInfo.Account = SelectedClient.Account;
                        ClientInfo.Type = SelectedClient.Type;
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
                        //var firstName = ClientFilter.FirstName.ToLowerInvariant();
                        //queryResult = queryResult.Where(client => client.FirstName.ToLowerInvariant().Contains(firstName));

                        queryResult = queryResult.Where(client => client.FirstName.IndexOf(ClientFilter.FirstName, StringComparison.CurrentCultureIgnoreCase) >= 0);
                    }
                    if (!string.IsNullOrEmpty(ClientFilter.LastName))
                    {
                        //var lastName = ClientFilter.LastName.ToLowerInvariant();
                        //queryResult = queryResult.Where(client => client.LastName.Contains(lastName));

                        queryResult = queryResult.Where(client => client.LastName.IndexOf(ClientFilter.LastName, StringComparison.CurrentCultureIgnoreCase) >= 0);
                    }
                    if (ClientFilter.Birthdate != null)
                    {
                        queryResult = queryResult.Where(client => client.Birthdate == ClientFilter.Birthdate);
                    }

                    if (ClientFilter.Type != ClientTypes.None)
                    {
                        queryResult = queryResult.Where(client => client.Type == ClientFilter.Type);
                    }


                    //if (!string.IsNullOrEmpty(ClientFilter.Account))
                    //{
                    //    //var account = ClientFilter.Account.ToLowerInvariant();
                    //    //queryResult = queryResult.Where(client => client.Account.Contains(account));

                    //    queryResult = queryResult.Where(client => client.Account.IndexOf(ClientFilter.Account, StringComparison.CurrentCultureIgnoreCase) >= 0);
                    //}
                    if (ClientFilter.Room != null)
                    {
                        queryResult = queryResult.Where(client => client.Room == ClientFilter.Room);
                    }

                    FilteredClientList = queryResult?.ToList();
                }));

        #endregion
    }
}