using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace EternityApp.Models
{
    public class ShellHeaderViewModel : INotifyPropertyChanged
    {
        private string _username;
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

        public ShellHeaderViewModel() { }

        private void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
