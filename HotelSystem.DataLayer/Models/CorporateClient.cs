using System;
using System.Collections.Generic;
using System.Text;

namespace HotelSystem.DataLayer.Models
{
    public class CorporateClient : Client
    {
        public CorporateClient()
        {
            Type = ClientTypes.CorporateClient;
        }

        private string _account;

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

        public override bool Equals(object obj)
        {
            if (obj is CorporateClient)
            {
                CorporateClient otherClient = obj as CorporateClient;
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

        public override int GetHashCode()
        {
            int hash = base.GetHashCode();

            unchecked // Overflow is fine, just wrap
            {
                // Suitable nullity checks etc, of course :)
                hash = hash * 6439 + Account?.GetHashCode() ?? 2515;
            }
            return hash;
        }
    }
}
