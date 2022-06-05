using EternityApp.Models;
using EternityApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EternityApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewPage : ContentPage
    {
        private readonly CityService _cityService;
        private readonly AttractionService _attractionService;
        private readonly ActionItemService _actionItemService;
        private readonly ImageService _imageService;
        public AsyncCommand<City> CityItemTappedCommand { get; }
        public AsyncCommand<Attraction> AttractionItemTappedCommand { get; }
        private IEnumerable<City> _citiesList;
        private IEnumerable<Attraction> _attractionsList;

        public ViewPage()
        {
            InitializeComponent();
            BindingContext = this;
            _cityService = new CityService();
            _attractionService = new AttractionService();
            _actionItemService = new ActionItemService();
            _imageService = new ImageService();
            CityItemTappedCommand = new AsyncCommand<City>(CityItemTapped);
            AttractionItemTappedCommand = new AsyncCommand<Attraction>(AttractionItemTapped);
            Routing.RegisterRoute("/CurrentCityPage", typeof(CurrentCityPage));
            Routing.RegisterRoute("/CurrentAttractionPage", typeof(CurrentAttractionPage));
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            BusyLayout.IsVisible = true;
            MainLayout.IsVisible = false;
            LoadingWheel.IsRunning = true;
            await LoadData();
            BusyLayout.IsVisible = false;
            MainLayout.IsVisible = true;
            LoadingWheel.IsRunning = false;
        }

        private async Task LoadData()
        {
            BindableLayout.SetItemsSource(CityPinsList, null);
            BindableLayout.SetItemsSource(AttractionPinsList, null);
            _citiesList = new List<City>();
            _attractionsList = new List<Attraction>();
            var cityPins = await _actionItemService.GetAction(1, 3);
            if (cityPins.Count() > 0)
            {
                var cities = await _cityService.Get();
                foreach (var item in cityPins)
                {
                    _citiesList = _citiesList.Append(cities.First(x => x.CityId == item.ItemId));
                }

                foreach (var item in _citiesList)
                {
                    item.TitleImagePath = $"{AppSettings.Url}images/cities/{item.CityId}/{await _imageService.GetTitleImage("cities", (int)item.CityId)}";
                }

                BindableLayout.SetItemsSource(CityPinsList, _citiesList);
            }

            var attractionPins = await _actionItemService.GetAction(2, 3);
            if (attractionPins.Count() > 0)
            {
                var attractions = await _attractionService.Get();
                foreach (var item in attractionPins)
                {
                    _attractionsList = _attractionsList.Append(attractions.First(x => x.AttractionId == item.ItemId));
                }

                foreach (var item in _attractionsList)
                {
                    item.TitleImagePath = $"{AppSettings.Url}images/attractions/{item.AttractionId}/{await _imageService.GetTitleImage("attractions", (int)item.AttractionId)}";
                }

                BindableLayout.SetItemsSource(AttractionPinsList, _attractionsList);
            }
        }

        private async Task CityItemTapped(City sender)
        {
            BusyLayout.IsVisible = true;
            MainLayout.IsVisible = false;
            LoadingWheel.IsRunning = true;
            await Shell.Current.GoToAsync($"/CurrentCityPage?id={(int)sender.CityId}");
            BusyLayout.IsVisible = false;
            MainLayout.IsVisible = true;
            LoadingWheel.IsRunning = false;
        }

        private async Task AttractionItemTapped(Attraction sender)
        {
            BusyLayout.IsVisible = true;
            MainLayout.IsVisible = false;
            LoadingWheel.IsRunning = true;
            await Shell.Current.GoToAsync($"/CurrentAttractionPage?id={(int)sender.AttractionId}");
            BusyLayout.IsVisible = false;
            MainLayout.IsVisible = true;
            LoadingWheel.IsRunning = false;
        }

        private async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            await LoadData();
            RefreshView.IsRefreshing = false;
        }
    }
}