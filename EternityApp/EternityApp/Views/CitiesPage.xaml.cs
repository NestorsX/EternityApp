using EternityApp.Models;
using EternityApp.Services;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EternityApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CitiesPage : ContentPage
    {
        private readonly CityService _cityService;
        private readonly ImageService _imageService;
        private IEnumerable<City> _citiesList;
        public CitiesPage()
        {
            InitializeComponent();
            _cityService = new CityService();
            _imageService = new ImageService();
            Routing.RegisterRoute("/InfoPage", typeof(InfoPage));
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            BusyLayout.IsVisible = true;
            MainLayout.IsVisible = false;
            LoadingWheel.IsRunning = true;
            if (_citiesList == null)
            {
                _citiesList = await _cityService.Get();
                foreach (var city in _citiesList)
                {
                    city.TitleImagePath = $"http://eternity.somee.com/images/cities/{city.CityId}/{await _imageService.GetTitleImage("cities", (int)city.CityId)}";
                }

                citiesList.ItemsSource = _citiesList;
            }

            BusyLayout.IsVisible = false;
            MainLayout.IsVisible = true;
            LoadingWheel.IsRunning = false;
        }

        private void citiesList_Refreshing(object sender, EventArgs e)
        {
            citiesList.IsRefreshing = true;
            citiesList.IsRefreshing = false;
        }

        private async void citiesList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await Shell.Current.GoToAsync($"/InfoPage?category=cities&id={(int)(e.Item as City).CityId}");
        }
    }
}