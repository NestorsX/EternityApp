using EternityApp.Models;
using EternityApp.Services;
using System;
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
        private readonly CityService _cityService;
        private readonly ImageService _imageService;
        private readonly ActionItemService _actionItemService;
        private Attraction _attraction;
        private City _city;
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
            _attraction = await _attractionService.Get(_id);
            Images = await _imageService.Get("attractions", _id);
            TitleLabel.Text = _attraction.Title;
            ImageCarousel.ItemsSource = Images;
            DescriptionLabel.Text = _attraction.Description;
            _isBookmarked = (await _actionItemService.GetAction(2, 1)).Any(x => x.ItemId == _attraction.AttractionId);
            _isPinned = (await _actionItemService.GetAction(2, 2)).Any(x => x.ItemId == _attraction.AttractionId);
            _isViewed = (await _actionItemService.GetAction(2, 3)).Any(x => x.ItemId == _attraction.AttractionId);
            PinButton.Source = _isPinned ? "icon_filledPin.png" : "icon_emptyPin.png";
            EyeButton.Source = _isViewed ? "icon_filledEye.png" : "icon_emptyEye.png";
            if (_attraction.Reference != null)
            {
                References.IsVisible = true;
                _city = (await _cityService.Get()).First(x => x.CityId == _attraction.Reference);
                CityReference.Text = _city.Title;
            }
            BusyLayout.IsVisible = false;
            MainLayout.IsVisible = true;
            LoadingWheel.IsRunning = false;
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            BusyLayout.IsVisible = true;
            MainLayout.IsVisible = false;
            LoadingWheel.IsRunning = true;
            if (_isBookmarked)
            {
                await _actionItemService.DeleteAction(2, 1, (int)_attraction.AttractionId);
                _isBookmarked = false;
                DependencyService.Get<IToast>().Show("Место удалено из закладок");
            }
            else
            {
                await _actionItemService.AddAction(2, 1, (int)_attraction.AttractionId);
                _isBookmarked = true;
                DependencyService.Get<IToast>().Show("Место добавлено в закладки");
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
                DependencyService.Get<IToast>().Show("Пока не хочу посещать это место");
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
                DependencyService.Get<IToast>().Show("Все-таки пока не увидел(а) это место :(");
            }
            else
            {
                _isViewed = true;
                await _actionItemService.AddAction(2, 3, (int)_attraction.AttractionId);
                DependencyService.Get<IToast>().Show("Я увидел(а) это место!");
            }

            EyeButton.Source = _isViewed ? "icon_filledEye.png" : "icon_emptyEye.png";
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"/CurrentCityPage?id={(int)_city.CityId}");
        }
    }
}