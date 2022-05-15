using EternityApp.Models;
using EternityApp.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EternityApp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected async override void OnStart()
        {
            base.OnStart();
            if (!Application.Current.Properties.TryGetValue("ID", out object _))
            {
                await Shell.Current.GoToAsync("//Login");
            }
            else
            {
                (Application.Current.MainPage as AppShell).ViewModel.Username = Application.Current.Properties["UserName"].ToString();
                await Shell.Current.GoToAsync("//MainPage");
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
