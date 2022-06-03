using EternityApp.Models;
using EternityApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EternityApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CityBookmarksPage : ContentPage
    {
        private readonly CityService _cityService;
        private readonly ImageService _imageService;
        private readonly ActionItemService _actionItemService;
        private IEnumerable<City> _citiesList;

        public CityBookmarksPage()
        {
            InitializeComponent();
            _cityService = new CityService();
            _imageService = new ImageService();
            _actionItemService = new ActionItemService();
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
            citiesList.ItemsSource = null;
            _citiesList = null;
            try
            {
                IEnumerable<DataAction> bookmarks = await _actionItemService.GetAction(1, 1);
                _citiesList = await _cityService.Get();
                var bookmarkedCities = new List<City>();
                foreach (var item in bookmarks)
                {
                    bookmarkedCities.Add(_citiesList.First(x => x.CityId == item.ItemId));
                }

                _citiesList = bookmarkedCities;
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
            await GetItemsList();
            citiesList.IsRefreshing = false;
        }

        private async void citiesList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await Shell.Current.GoToAsync($"/CurrentCityPage?id={(int)(e.Item as City).CityId}");
        }
    }
}