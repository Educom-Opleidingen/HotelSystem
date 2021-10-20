using HotelSystem.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HotelSystem.Model
{
    public class BaseModel : INotifyPropertyChanged
    {
        private int _id;
        public event PropertyChangedEventHandler PropertyChanged;


        public int Id
        {
            get => _id;
            set
            {
                if (value == _id) return;
                _id = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}