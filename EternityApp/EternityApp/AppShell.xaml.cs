using EternityApp.Models;

namespace EternityApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public ShellHeaderViewModel ViewModel { get; set; }
        public AppShell()
        {
            InitializeComponent();
            ViewModel = new ShellHeaderViewModel();
            BindingContext = ViewModel;
        }
    }
}
