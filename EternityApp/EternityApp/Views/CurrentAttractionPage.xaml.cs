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
    public partial class CurrentAttractionPage : ContentPage
    {
        private readonly AttractionService _attractionService;
        private readonly ImageService _imageService;
        private readonly BookmarkService _bookmarkService;
        private Attraction _attraction;
        private int _id;
        private AttractionBookmark _bookmark;
        private bool IsBookmarked { get; set; }

        public new int Id
        {
            set
            {
                _id = value;
            }
        }

        public IEnumerable<Models.Image> Images { get; set; }

        public CurrentAttractionPage()
        {
            InitializeComponent();
            _attractionService = new AttractionService();
            _imageService = new ImageService();
            _bookmarkService = new BookmarkService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            BusyLayout.IsVisible = true;
            MainLayout.IsVisible = false;
            LoadingWheel.IsRunning = true;
            _attraction = await _attractionService.Get(_id);
            Images = await _imageService.Get("attractions", _id);
            TitleLabel.Text = _attraction.Title;
            ImageCarousel.ItemsSource = Images;
            DescriptionLabel.Text = _attraction.Description;
            _bookmark = (await _bookmarkService.GetAttractionBookmarkList((int)Application.Current.Properties["id"])).FirstOrDefault(x => x.AttractionId == _id);
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
                await _bookmarkService.DeleteAttractionBookmark((int)_bookmark.AttractionBookmarkId);
                IsBookmarked = false;
            }
            else
            {
                await _bookmarkService.AddAttractionBookmark(new AttractionBookmark
                {
                    AttractionBookmarkId = null,
                    UserId = (int)Application.Current.Properties["id"],
                    AttractionId = _id,
                });

                _bookmark = (await _bookmarkService.GetAttractionBookmarkList((int)Application.Current.Properties["id"])).FirstOrDefault(x => x.AttractionId == _id);
                IsBookmarked = true;
            }

            BusyLayout.IsVisible = false;
            MainLayout.IsVisible = true;
            LoadingWheel.IsRunning = false;
        }
    }
}