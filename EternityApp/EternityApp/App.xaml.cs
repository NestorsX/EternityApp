using EternityApp.Services;
using Xamarin.Forms;
using Xamarin.Essentials;

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
            if (await SecureStorage.GetAsync("ID") != null)
            {
                (Application.Current.MainPage as AppShell).ViewModel.Username = await SecureStorage.GetAsync("Username");
                if (await SecureStorage.GetAsync("ImageUri") != null)
                {
                    (Application.Current.MainPage as AppShell).ViewModel.ImageSource = await SecureStorage.GetAsync("ImageUri");
                }
                else
                {
                    (Application.Current.MainPage as AppShell).ViewModel.ImageSource = "icon_no_avatar.png";
                }

                await Shell.Current.GoToAsync("//MainPage");
            }
            else
            {
                await Shell.Current.GoToAsync("//Login");
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
