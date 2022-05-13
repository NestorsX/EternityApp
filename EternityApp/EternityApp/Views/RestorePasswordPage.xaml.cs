using EternityApp.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EternityApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RestorePasswordPage : ContentPage
    {
        private readonly UserService _userService;
        public RestorePasswordPage()
        {
            InitializeComponent();
            _userService = new UserService();
            GoBackLabel.Text = "Пароль сброшен.\nНа почту выслан временный пароль.";
        }

        private async void RestoreButton_Clicked(object sender, EventArgs e)
        {
            BusyLayout.IsVisible = true;
            MainLayout.IsVisible = false;
            LoadingWheel.IsRunning = true;
            if (Email.Text != null)
            {
                try
                {
                    await _userService.RestorePassword(Email.Text);
                    BusyLayout.IsVisible = false;
                    GoBackLayout.IsVisible = true;
                    LoadingWheel.IsRunning = false;
                }
                catch
                {
                    ErrorLabel.Text = "Пользователя с таким email не существует";
                    ErrorLabel.IsVisible = true;
                    BusyLayout.IsVisible = false;
                    MainLayout.IsVisible = true;
                    LoadingWheel.IsRunning = false;
                }
            }
            else
            {
                ErrorLabel.Text = "Заполните поле Email";
                ErrorLabel.IsVisible = true;
                BusyLayout.IsVisible = false;
                MainLayout.IsVisible = true;
                LoadingWheel.IsRunning = false;
            }
        }
    }
}