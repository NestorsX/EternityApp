using EternityApp.Models;
using EternityApp.Services;
using System;
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
            _user = await _userService.Get(Convert.ToInt32(await SecureStorage.GetAsync("ID")));
            Email.Text = _user.Email;
            Username.Text = _user.UserName;
            if (!string.IsNullOrWhiteSpace(await SecureStorage.GetAsync("ImageUri")))
            {
                Image.Source = await SecureStorage.GetAsync("ImageUri");
            }
            else
            {
                Image.Source = "icon_no_avatar.png";
            }

            BusyLayout.IsVisible = false;
            LoadingWheel.IsRunning = false;
            MainLayout.IsVisible = true;
        }

        private async void UserImage_Clicked(object sender, EventArgs e)
        {
            BusyLayout.IsVisible = true;
            LoadingWheel.IsRunning = true;
            MainLayout.IsVisible = false;
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Выберите фото"
            });

            if (result != null) 
            {
                var content = new MultipartFormDataContent
                {
                    { new StreamContent(await result.OpenReadAsync()), "image", result.FileName }
                };

                await _imageService.PostUserImage((int)_user.UserId, content);
                try
                {
                    await SecureStorage.SetAsync("ImageUri", $"{AppSettings.Url}images/users/{(int)_user.UserId}/{await _imageService.GetTitleImage("users", (int)_user.UserId)}");
                    Image.Source = await SecureStorage.GetAsync("ImageUri");
                    (Application.Current.MainPage as AppShell).ViewModel.ImageSource = await SecureStorage.GetAsync("ImageUri");
                }
                catch
                {
                    Image.Source = "icon_no_avatar.png";
                    (Application.Current.MainPage as AppShell).ViewModel.ImageSource = "icon_no_avatar.png";
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

                await SecureStorage.SetAsync("Username", newUserName);
                (Application.Current.MainPage as AppShell).ViewModel.Username = newUserName;
            }
            catch (ArgumentException ex)
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
            SecureStorage.RemoveAll();
            Application.Current.MainPage = new AppShell();
            await Shell.Current.GoToAsync("//Login");
            BusyLayout.IsVisible = false;
            LoadingWheel.IsRunning = false;
            MainLayout.IsVisible = true;
        }
    }
}