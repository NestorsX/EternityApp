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
    public partial class AttractionsPage : ContentPage
    {
        private readonly AttractionService _attractionService;
        private readonly ImageService _imageService;
        private IEnumerable<Attraction> _attractionsList;
        private bool _isSearching;
        public AttractionsPage()
        {
            InitializeComponent();
            _attractionService = new AttractionService();
            _imageService = new ImageService();
            Routing.RegisterRoute("/CurrentAttractionPage", typeof(CurrentAttractionPage));
        }

        protected async override void OnAppearing()
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
            attractionsList.ItemsSource = null;
            _attractionsList = null;
            try
            {
                _attractionsList = await _attractionService.Get();
                foreach (var item in _attractionsList)
                {
                    item.TitleImagePath = $"{AppSettings.Url}images/attractions/{item.AttractionId}/{await _imageService.GetTitleImage("attractions", (int)item.AttractionId)}";
                }

                attractionsList.ItemsSource = _attractionsList;
            }
            catch
            {
                NoData.IsVisible = true;
            }
        }

        private async void attractionsList_Refreshing(object sender, EventArgs e)
        {
            attractionsList.IsRefreshing = true;
            if (!_isSearching)
            {
                await GetItemsList();
            }

            attractionsList.IsRefreshing = false;
        }

        private async void attractionsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await Shell.Current.GoToAsync($"/CurrentAttractionPage?id={(int)(e.Item as Attraction).AttractionId}");
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            _isSearching = !string.IsNullOrWhiteSpace(e.NewTextValue);
            attractionsList.ItemsSource = _attractionsList.Where(x => x.Title.ToLower().Contains(e.NewTextValue.ToLower())).ToList();
        }
    }
}