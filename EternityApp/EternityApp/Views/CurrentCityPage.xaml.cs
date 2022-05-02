using EternityApp.Models;
using EternityApp.Services;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EternityApp.Views
{
    [QueryProperty(nameof(Id), "id")]
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class CurrentCityPage : ContentPage
    {
        private readonly CityService _cityService;
        private readonly ImageService _imageService;
        private City _city;
        private int _id;

        public new int Id
        {
            set
            {
                _id = value;
            }
        }

        public IEnumerable<Models.Image> Images { get; set; }

        public CurrentCityPage()
        {
            InitializeComponent();
            _cityService = new CityService();
            _imageService = new ImageService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            BusyLayout.IsVisible = true;
            MainLayout.IsVisible = false;
            LoadingWheel.IsRunning = true;
            _city = await _cityService.Get(_id);
            Images = await _imageService.Get("cities", _id);
            TitleLabel.Text = _city.Title;
            ImageCarousel.ItemsSource = Images;
            DescriptionLabel.Text = _city.Description;
            BusyLayout.IsVisible = false;
            MainLayout.IsVisible = true;
            LoadingWheel.IsRunning = false;
        }
    }
}