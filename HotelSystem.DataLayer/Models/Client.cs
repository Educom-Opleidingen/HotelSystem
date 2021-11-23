namespace HotelSystem.DataLayer.Models
{
    public class Client : Person
    {
        private string _account;
        private Room _room;

        public string Account
        {
            get => _account;
            set
            {
                if (value == _account) return;
                _account = value;
                OnPropertyChanged();
            }
        }

        public virtual Room Room
        {
            get => _room;
            set
            {
                if (Equals(value, _room)) return;
                _room = value;
                OnPropertyChanged();
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Client)
            {
                Client otherClient = obj as Client;
                if (otherClient == null)
                {
                    return false;
                }

                if (!base.Equals(obj))
                {
                    return false;
                }

                if (otherClient.Account != Account)
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

        public bool EqualsIncludingRoom(Client otherClient)
        {
            if (!Equals(otherClient))
            {
                return false;
            }

            if (!Equals(Room, otherClient.Room))
            {
                return false;
            }

            return true;
        }
        public override int GetHashCode()
        {
            int hash = base.GetHashCode();

            unchecked // Overflow is fine, just wrap
            {
                // Suitable nullity checks etc, of course :)
                hash = hash * 6439 + Account?.GetHashCode() ?? 2515;
                hash = hash * 973 + Room?.GetHashCode() ?? 9663;
            }
            return hash;
        }
    }
}