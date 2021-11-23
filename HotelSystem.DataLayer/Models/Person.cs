using System;

namespace HotelSystem.DataLayer.Models
{
    public class Person : BaseModel
    {
        private string _firstName;
        private string _lastName;
        private DateTime? _birthdate;

        public string FirstName
        {
            get => _firstName;
            set
            {
                if (value == _firstName) return;
                _firstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                if (value == _lastName) return;
                _lastName = value;
                OnPropertyChanged();
            }
        }

        public DateTime? Birthdate
        {
            get => _birthdate;
            set
            {
                if (value.Equals(_birthdate)) return;
                _birthdate = value;
                OnPropertyChanged();
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Person)
            {
                Person otherPerson = obj as Person;
                if (otherPerson == null)
                {
                    return false;
                }

                if (otherPerson.FirstName != FirstName || otherPerson.LastName != LastName || otherPerson.Birthdate != Birthdate)
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
                hash = hash * 5039 + FirstName?.GetHashCode() ?? 4615;
                hash = hash * 883 + LastName?.GetHashCode() ?? 5863;
                hash = hash * 9719 + Birthdate.GetHashCode();
            }
            return hash;
        }
    }
}