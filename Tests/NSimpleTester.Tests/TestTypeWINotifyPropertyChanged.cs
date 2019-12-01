using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NSimpleTester.Tests
{
    public class TestTypeWINotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _propertyOne;
        public int PropertyOne
        {
            get { return _propertyOne; }
            set
            {
                _propertyOne = value;
                OnPropertyChanged();
            }
        }

        private int _propertyTwo;
        public int PropertyTwo
        {
            get { return _propertyTwo; }
            set
            {
                _propertyTwo = value;
                OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
