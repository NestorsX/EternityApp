using EternityApp.Models;
using EternityApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EternityApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            BusyLayout.IsVisible = true;
            MainLayout.IsVisible = false;
            ErrorLabel.IsVisible = false;
            LoadingWheel.IsRunning = true;
            var userService = new UserService();
            if (Username.Text != null && Password.Text != null)
            {
                try
                {
                    User currentUser = await userService.Add(new User()
                    {
                        UserId = null,
                        UserName = Username.Text,
                        Email = Email.Text,
                        Password = Password.Text,
                        RoleId = 2
                    });
                    
                    await SecureStorage.SetAsync("ID", currentUser.UserId.ToString());
                    await SecureStorage.SetAsync("Username", currentUser.UserName);
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
    }
}