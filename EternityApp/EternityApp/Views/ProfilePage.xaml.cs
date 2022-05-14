using EternityApp.Interfaces;
using EternityApp.Services;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EternityApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        private readonly ImageService _imageService;

        public ProfilePage()
        {
            InitializeComponent();
            _imageService = new ImageService();
        }

        private async void UserImage_Clicked(object sender, EventArgs e)
        {
            BusyLayout.IsVisible = true;
            MainLayout.IsVisible = false;
            Stream stream = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();
            if (stream != null)
            {
                var fileContent = new StreamContent(stream);
                var content = new MultipartFormDataContent
                {
                    { fileContent, "file", "avatar.png" }
                };

                await _imageService.PostUserImage((int)Application.Current.Properties["id"], content);
                //Image.Source = await _imageService.GetTitleImage("users", (int)Application.Current.Properties["id"]);
            }

            BusyLayout.IsVisible = false;
            MainLayout.IsVisible = true;
        }
    }
}