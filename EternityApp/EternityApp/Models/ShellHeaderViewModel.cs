using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace EternityApp.Models
{
    public class ShellHeaderViewModel : INotifyPropertyChanged
    {
        private string _username;
        private ImageSource _imageSource;
        public event PropertyChangedEventHandler PropertyChanged;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public ImageSource ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                OnPropertyChanged();
            }
        }

        public ShellHeaderViewModel() { }

        private void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
