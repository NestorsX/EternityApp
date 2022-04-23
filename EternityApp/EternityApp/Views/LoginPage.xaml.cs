using EternityApp.Models;
using EternityApp.Services;
using System;
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
            Routing.RegisterRoute("//RegisterPage", typeof(RegisterPage));
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            BusyLayout.IsVisible = true;
            MainLayout.IsEnabled = false;
            ErrorLabel.IsVisible = false;
            LoadingWheel.IsRunning = true;
            var userService = new UserService();
            if (Username.Text != null && Password.Text != null)
            {
                User currentUser = await userService.Get(Username.Text, Password.Text);
                if (currentUser != null)
                {
                    Application.Current.Properties["id"] = currentUser.UserId;
                    await Shell.Current.GoToAsync("//MainPage");
                }
                else
                {
                    ErrorLabel.IsVisible = true;
                }
            }

            BusyLayout.IsVisible = false;
            MainLayout.IsEnabled = true;
            LoadingWheel.IsRunning = false;
        }

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(RegisterPage)}");
        }
    }
}