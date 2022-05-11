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
    public partial class AttractionBookmarksPage : ContentPage
    {
        private readonly AttractionService _attractionService;
        private readonly ImageService _imageService;
        private readonly BookmarkService _bookmarkService;
        private IEnumerable<Attraction> _attractionsList;

        public AttractionBookmarksPage()
        {
            InitializeComponent();
            _attractionService = new AttractionService();
            _imageService = new ImageService();
            _bookmarkService = new BookmarkService();
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
                IEnumerable<AttractionBookmark> bookmarks = await _bookmarkService.GetAttractionBookmarkList((int)Application.Current.Properties["id"]);
                _attractionsList = await _attractionService.Get();
                var bookmarkedCities = new List<Attraction>();
                foreach (var item in bookmarks)
                {
                    bookmarkedCities.Add(_attractionsList.First(x => x.AttractionId == item.AttractionId));
                }

                _attractionsList = bookmarkedCities;
                foreach (var item in _attractionsList)
                {
                    item.TitleImagePath = $"http://eternity.somee.com/images/attractions/{item.AttractionId}/{await _imageService.GetTitleImage("attractions", (int)item.AttractionId)}";
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
            await GetItemsList();
            attractionsList.IsRefreshing = false;
        }

        private async void attractionsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await Shell.Current.GoToAsync($"/CurrentAttractionPage?id={(int)(e.Item as Attraction).AttractionId}");
        }
    }
}