using System;

namespace HotelSystem.Model
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


    }
}