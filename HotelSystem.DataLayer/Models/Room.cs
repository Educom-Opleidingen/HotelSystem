using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using HotelSystem.Annotations;

namespace HotelSystem.Model
{
    public enum RoomTypes
    {
        None,
        StandardRoom,
        BusinessClassRoom,
        JuniorSuite,
        PresidentialSuite
    }

    public class Room : BaseModel
    {
        private RoomTypes _type;
        private string _number;
        private IList<Client> _clients;

        public string Number
        {
            get => _number;
            set
            {
                if (value == _number) return;
                _number = value;
                OnPropertyChanged();
            }
        }

        public RoomTypes Type
        {
            get => _type;
            set
            {
                if (value == _type) return;

                _type = value;
                OnPropertyChanged();
            }
        }

        public virtual IList<Client> Clients
        {
            get => _clients;
            set
            {
                if (Equals(value, _clients)) return;
                _clients = value;
                OnPropertyChanged();
            }
        }

        public Room()
        {
            _clients = new List<Client>();
        }
    }
}