using System;
using System.Collections.Generic;
using System.Text;

namespace HotelSystem.DataLayer.Models
{
    public class PrivateClient : Client
    {
        private string _emailAddress;
        private string _phoneNumber;

        public PrivateClient()
        {
            Type = ClientTypes.PrivateClient;
        }

        public string EmailAddress
        {
            get => _emailAddress;
            set
            {
                if (_emailAddress != value)
                {
                    _emailAddress = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber; 
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is PrivateClient)
            {
                PrivateClient otherClient = obj as PrivateClient;
                if (otherClient == null)
                {
                    return false;
                }

                if (!base.Equals(obj))
                {
                    return false;
                }

                if (otherClient.EmailAddress != EmailAddress)
                {
                    return false;
                }

                if (otherClient._phoneNumber != PhoneNumber) 
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
                hash = hash * 6896 + EmailAddress?.GetHashCode() ?? 7589;
                hash = hash * 4586 + PhoneNumber?.GetHashCode() ?? 1658;

            }
            return hash;
        }
    }
}
