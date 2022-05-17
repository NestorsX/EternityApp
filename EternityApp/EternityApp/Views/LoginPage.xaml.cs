using EternityApp.Models;
using EternityApp.Services;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EternityApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            Routing.RegisterRoute("RegisterPage", typeof(RegisterPage));
            Routing.RegisterRoute("RestorePasswordPage", typeof(RestorePasswordPage));
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            BusyLayout.IsVisible = true;
            MainLayout.IsVisible = false;
            ErrorLabel.IsVisible = false;
            LoadingWheel.IsRunning = true;
            var userService = new UserService();
            var imageService = new ImageService();
            if (Username.Text != null && Password.Text != null)
            {
                try
                {
                    User currentUser = await userService.Get(Username.Text, Password.Text);
                    await SecureStorage.SetAsync("ID", currentUser.UserId.ToString());
                    await SecureStorage.SetAsync("Username", currentUser.UserName);
                    (Application.Current.MainPage as AppShell).ViewModel.Username = currentUser.UserName;
                    try
                    {
                        await SecureStorage.SetAsync("ImageUri", $"http://eternity.somee.com/images/users/{(int)currentUser.UserId}/{await imageService.GetTitleImage("users", (int)currentUser.UserId)}");

                        (Application.Current.MainPage as AppShell).ViewModel.ImageSource = await SecureStorage.GetAsync("ImageUri");
                    }
                    catch
                    {
                        (Application.Current.MainPage as AppShell).ViewModel.ImageSource = "icon_no_avatar.png";
                    }

                    await Shell.Current.GoToAsync("//MainPage");
                }
                catch
                {
                    ErrorLabel.IsVisible = true;
                }
            }

            BusyLayout.IsVisible = false;
            MainLayout.IsVisible = true;
            LoadingWheel.IsRunning = false;
        }

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(RegisterPage)}");
        }

        private async void RestorePassword_Tapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(RestorePasswordPage)}");
        }
    }
}