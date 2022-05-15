using EternityApp.Interfaces;
using EternityApp.Models;
using EternityApp.Services;
using System;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EternityApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        private readonly ImageService _imageService;
        private readonly UserService _userService;
        private User _user;

        public ProfilePage()
        {
            InitializeComponent();
            _imageService = new ImageService();
            _userService = new UserService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            BusyLayout.IsVisible = true;
            LoadingWheel.IsRunning = true;
            MainLayout.IsVisible = false;
            try
            {
                Image.Source = await _imageService.GetTitleImage("users", (int)Application.Current.Properties["ID"]);
            }
            catch
            {
                Image.Source = ImageSource.FromFile("icon_no_avatar.png");
            }

            _user = await _userService.Get((int)Application.Current.Properties["ID"]);
            Email.Text = _user.Email;
            Username.Text = _user.UserName;

            BusyLayout.IsVisible = false;
            LoadingWheel.IsRunning = false;
            MainLayout.IsVisible = true;
        }

        private async void UserImage_Clicked(object sender, EventArgs e)
        {
            BusyLayout.IsVisible = true;
            LoadingWheel.IsRunning = true;
            MainLayout.IsVisible = false;
            Stream stream = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();
            if (stream != null)
            {
                var fileContent = new StreamContent(stream);
                var content = new MultipartFormDataContent
                {
                    { fileContent, "file", "avatar.png" }
                };

                await _imageService.PostUserImage((int)Application.Current.Properties["ID"], content);
                try
                {
                    Image.Source = await _imageService.GetTitleImage("users", (int)Application.Current.Properties["ID"]);
                }
                catch
                {
                    Image.Source = ImageSource.FromFile("icon_no_avatar.png");
                }
            }

            BusyLayout.IsVisible = false;
            LoadingWheel.IsRunning = false;
            MainLayout.IsVisible = true;
        }

        private async void SaveChanges_Clicked(object sender, EventArgs e)
        {
            BusyLayout.IsVisible = true;
            LoadingWheel.IsRunning = true;
            MainLayout.IsVisible = false;
            EmailErrorLabel.IsVisible = false;
            UsernameErrorLabel.IsVisible = false;
            PasswordErrorLabel.IsVisible = false;
            ErrorLabel.IsVisible = false;
            string newEmail = null;
            string newUserName = null;
            string newPassword = null;
            if (!string.IsNullOrEmpty(Email.Text) && Regex.IsMatch(Email.Text, @"^((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\s*)+$"))
            {
                newEmail = Email.Text;
            }
            else
            {
                EmailErrorLabel.IsVisible = true;
                EmailErrorLabel.Text = "Некорректный email";
            }

            if (!string.IsNullOrEmpty(Username.Text))
            {
                newUserName = Username.Text;
            }
            else
            {
                UsernameErrorLabel.IsVisible = true;
                UsernameErrorLabel.Text = "Введите логин";
            }

            if (!string.IsNullOrEmpty(OldPassword.Text))
            {
                if (!string.IsNullOrEmpty(NewPassword.Text) && string.Equals(NewPassword.Text, NewPassword2.Text))
                {
                    newPassword = NewPassword.Text;
                }
                else
                {
                    PasswordErrorLabel.IsVisible = true;
                    PasswordErrorLabel.Text = "Проверьте введенный пароль";
                }
            }

            try
            {
                await _userService.Update(new User
                {
                    UserId = _user.UserId,
                    Email = newEmail,
                    UserName = newUserName,
                    Password = newPassword ?? _user.Password,
                    RoleId = 0
                });
            }
            catch (Exception ex)
            {
                ErrorLabel.IsVisible = true;
                ErrorLabel.Text = ex.Message;
            }

            BusyLayout.IsVisible = false;
            LoadingWheel.IsRunning = false;
            MainLayout.IsVisible = true;
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            BusyLayout.IsVisible = true;
            LoadingWheel.IsRunning = true;
            MainLayout.IsVisible = false;
            Application.Current.Properties.Remove("ID");
            Application.Current.MainPage = new AppShell();
            await Shell.Current.GoToAsync("//Login");
            BusyLayout.IsVisible = false;
            LoadingWheel.IsRunning = false;
            MainLayout.IsVisible = true;
        }
    }
}