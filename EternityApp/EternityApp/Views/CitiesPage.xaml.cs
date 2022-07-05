using EternityApp.Models;
using EternityApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private bool _isSearching;
        public CitiesPage()
        {
            InitializeComponent();
            _cityService = new CityService();
            _imageService = new ImageService();
            _isSearching = false;
            Routing.RegisterRoute("/CurrentCityPage", typeof(CurrentCityPage));
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            BusyLayout.IsVisible = true;
            MainLayout.IsVisible = false;
            LoadingWheel.IsRunning = true;
            await GetItemsList();
            BusyLayout.IsVisible = false;
            MainLayout.IsVisible = true;
            LoadingWheel.IsRunning = false;
        }

        private async Task GetItemsList()
        {
            NoData.IsVisible = false;
            try
            {
                _citiesList = await _cityService.Get();
                foreach (var item in _citiesList)
                {
                    item.TitleImagePath = $"{AppSettings.Url}images/cities/{item.CityId}/{await _imageService.GetTitleImage("cities", (int)item.CityId)}";
                }

                citiesList.ItemsSource = _citiesList;
            }
            catch
            {
                NoData.IsVisible = true;
            }
        }

        private async void citiesList_Refreshing(object sender, EventArgs e)
        {
            citiesList.IsRefreshing = true;
            if (!_isSearching)
            {
                await GetItemsList();
            }

            citiesList.IsRefreshing = false;
        }

        private async void citiesList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await Shell.Current.GoToAsync($"/CurrentCityPage?id={(int)(e.Item as City).CityId}");
        }

        // поиск
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e) 
        {
            _isSearching = !string.IsNullOrWhiteSpace(e.NewTextValue);
            citiesList.ItemsSource = _citiesList.Where(x => x.Title.ToLower().Contains(e.NewTextValue.ToLower())).ToList();
        }
    }
}