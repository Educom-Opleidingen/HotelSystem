using System.Collections.Generic;

namespace HotelSystem.DataLayer.Models
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

        public override bool Equals(object obj)
        {
            if (obj is Room)
            {
                Room otherRoom = obj as Room;
                if (otherRoom == null)
                {
                    return false;
                }

                if (otherRoom.Number != Number)
                {
                    return false;
                }

                if (otherRoom.Type != Type)
                {
                    return false;
                }

                return true;
            }
            else 
            { 
                return false; 
            }
        }
        public override int GetHashCode()
        {
            int hash = base.GetHashCode();

            unchecked // Overflow is fine, just wrap
            {
                // Suitable nullity checks etc, of course :)
                hash = hash * 839 + Number?.GetHashCode() ?? 2515;
                hash = hash * 273 + Type.GetHashCode() ;
            }
            return hash;
        }
    }
}