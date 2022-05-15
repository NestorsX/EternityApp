using EternityApp.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EternityApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public ShellHeaderViewModel ViewModel { get; set; }
        public AppShell()
        {
            InitializeComponent();
            ViewModel = new ShellHeaderViewModel();
            this.BindingContext = ViewModel;
        }
    }
}
