using HotelSystem.DataLayer;
using HotelSystem.HotelDbContext;
using HotelSystem.Model;
using HotelSystem.View;
using HotelSystem.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Configuration;
using System.Linq;
using System.Windows;

namespace HotelSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HotelContext Context { get; }

        public MainWindow()
        {
            InitializeComponent();

            Context = new HotelContext(ConfigurationManager.ConnectionStrings["HotelDbConnectionString"].ConnectionString);
            Context.Database.Migrate();

            if (!Context.Rooms.Any())
            {
                Fill(); // Fill database with default values
            }
            ClientRepository clientRepository = new ClientRepository(Context);
            RoomRepository roomRepository = new RoomRepository(Context);
            StandardDialog standardDialog = new StandardDialog();

            ClientsTab.DataContext = new ClientsTabViewModel(clientRepository, roomRepository, standardDialog);
            RoomsTab.DataContext = new RoomsTabViewModel(roomRepository);
        }

        private void Fill()
        {
            var rooms = new[]
            {
                new Room {Number = "1", Type = RoomTypes.StandardRoom},
                new Room {Number = "2", Type = RoomTypes.JuniorSuite},
                new Room {Number = "3", Type = RoomTypes.StandardRoom},
                new Room {Number = "4", Type = RoomTypes.PresidentialSuite},
                new Room {Number = "5", Type = RoomTypes.JuniorSuite},
                new Room {Number = "6", Type = RoomTypes.StandardRoom},
                new Room {Number = "7", Type = RoomTypes.PresidentialSuite}
            };

            var clients = new[]
            {
                new Client {FirstName = "Stanislav", LastName = "Herasymiuk", Birthdate = new DateTime(1995, 9, 2), Account = "stas_the_best", Room = rooms[1]},
                new Client {FirstName = "Bob", LastName = "Marley", Birthdate = new DateTime(1952, 3, 25), Account = "919191", Room = rooms[3]},
                new Client {FirstName = "Frank", LastName = "Sinatra", Birthdate = new DateTime(1957, 7, 3), Account = "100500", Room = rooms[3]},
                new Client {FirstName = "Phill", LastName = "Colson", Birthdate = new DateTime(1966, 12, 6), Account = "S.H.I.E.L.D.", Room = rooms[5]},
                new Client {FirstName = "Dayzee", LastName = "Skay", Birthdate = new DateTime(1989, 10, 30), Account = "Hydra", Room = rooms[4]},
                new Client {FirstName = "Elvis", LastName = "Presley", Birthdate = new DateTime(1960, 2, 17), Account = "YA krevedko", Room = rooms[6]}
            };

            Context.Rooms.AddRange(rooms);
            Context.Clients.AddRange(clients);
            Context.SaveChanges();
        }
    }
}