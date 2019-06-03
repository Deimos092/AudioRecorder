using System.ComponentModel;
using System.Windows;

namespace AudioRecorder.ViewModel
{
    class BaseViewModel : DependencyObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        internal void OnPropertyChanged(string propertyName, object sender = null)
        {
            PropertyChanged?.Invoke(sender ?? this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
