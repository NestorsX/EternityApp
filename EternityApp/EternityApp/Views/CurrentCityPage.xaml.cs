using EternityApp.Models;
using EternityApp.Services;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EternityApp.Views
{
    [QueryProperty(nameof(Id), "id")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CurrentCityPage : ContentPage
    {
        private readonly CityService _cityService;
        private readonly ImageService _imageService;
        private readonly BookmarkService _bookmarkService;
        private City _city;
        private int _id;
        private CityBookmark _bookmark;
        private bool IsBookmarked { get; set; }

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
            _bookmarkService = new BookmarkService();
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
            _bookmark = (await _bookmarkService.GetCityBookmarkList((int)Application.Current.Properties["id"])).FirstOrDefault(x => x.CityId == _id);
            IsBookmarked = false;
            if (_bookmark != null)
            {
                IsBookmarked = true;
            }

            BusyLayout.IsVisible = false;
            MainLayout.IsVisible = true;
            LoadingWheel.IsRunning = false;
        }

        private async void ToolbarItem_Clicked(object sender, System.EventArgs e)
        {
            BusyLayout.IsVisible = true;
            MainLayout.IsVisible = false;
            LoadingWheel.IsRunning = true;
            if (IsBookmarked)
            {
                await _bookmarkService.DeleteCityBookmark((int)_bookmark.CityBookmarkId);
                IsBookmarked = false;
            }
            else
            {
                await _bookmarkService.AddCityBookmark(new CityBookmark
                {
                    CityBookmarkId = null,
                    UserId = (int)Application.Current.Properties["id"],
                    CityId = _id,
                });

                _bookmark = (await _bookmarkService.GetCityBookmarkList((int)Application.Current.Properties["id"])).FirstOrDefault(x => x.CityId == _id);
                IsBookmarked = true;
            }

            BusyLayout.IsVisible = false;
            MainLayout.IsVisible = true;
            LoadingWheel.IsRunning = false;
        }
    }
}