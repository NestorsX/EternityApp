using EternityApp.Models;
using EternityApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
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
        private readonly ActionItemService _actionItemService;
        private Attraction _attraction;
        private int _id;
        private bool _isBookmarked;
        private bool _isPinned;
        private bool _isViewed;

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
            _actionItemService = new ActionItemService();
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
            _isBookmarked = (await _actionItemService.GetAction(2, 1)).Any(x => x.ItemId == _attraction.AttractionId);
            _isPinned = (await _actionItemService.GetAction(2, 2)).Any(x => x.ItemId == _attraction.AttractionId);
            _isViewed = (await _actionItemService.GetAction(2, 3)).Any(x => x.ItemId == _attraction.AttractionId);
            BusyLayout.IsVisible = false;
            MainLayout.IsVisible = true;
            LoadingWheel.IsRunning = false;
        }

        private async void ToolbarItem_Clicked(object sender, System.EventArgs e)
        {
            BusyLayout.IsVisible = true;
            MainLayout.IsVisible = false;
            LoadingWheel.IsRunning = true;
            if (_isBookmarked)
            {
                await _actionItemService.DeleteAction(2, 1, (int)_attraction.AttractionId);
                _isBookmarked = false;
            }
            else
            {
                await _actionItemService.AddAction(2, 1, (int)_attraction.AttractionId);
                _isBookmarked = true;
            }

            BusyLayout.IsVisible = false;
            MainLayout.IsVisible = true;
            LoadingWheel.IsRunning = false;
        }

        private async void PinButton_Tapped(object sender, EventArgs e)
        {
            if (_isPinned)
            {
                _isPinned = false;
                await _actionItemService.DeleteAction(2, 2, (int)_attraction.AttractionId);
                DependencyService.Get<IToast>().Show("Хочу посетить это место");
            }
            else
            {
                _isPinned = true;
                await _actionItemService.AddAction(2, 2, (int)_attraction.AttractionId);
                DependencyService.Get<IToast>().Show("Хочу посетить это место");
            }

            PinButton.Source = _isPinned ? "icon_filledPin.png" : "icon_emptyPin.png";
        }

        private async void EyeButton_Tapped(object sender, EventArgs e)
        {
            if (_isViewed)
            {
                _isViewed = false;
                await _actionItemService.DeleteAction(2, 3, (int)_attraction.AttractionId);
                DependencyService.Get<IToast>().Show("Видел(а) это место");
            }
            else
            {
                _isViewed = true;
                await _actionItemService.AddAction(2, 3, (int)_attraction.AttractionId);
                DependencyService.Get<IToast>().Show("Не видел(а) это место");
            }

            EyeButton.Source = _isViewed ? "icon_filledEye.png" : "icon_emptyEye.png";
        }
    }
}