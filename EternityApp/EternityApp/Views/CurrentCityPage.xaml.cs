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
    [QueryProperty(nameof(Id), "id")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CurrentCityPage : ContentPage
    {
        private readonly CityService _cityService;
        private readonly AttractionService _attractionService;
        private readonly ImageService _imageService;
        private readonly ActionItemService _actionItemService;
        public AsyncCommand<Attraction> ItemTappedCommand { get; }
        private City _city;
        private IEnumerable<Attraction> _attractions;
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

        public CurrentCityPage()
        {
            InitializeComponent();
            BindingContext = this;
            _cityService = new CityService();
            _attractionService = new AttractionService();
            _imageService = new ImageService();
            _actionItemService = new ActionItemService();
            ItemTappedCommand = new AsyncCommand<Attraction>(Reference_Tapped);
            Routing.RegisterRoute("/CurrentAttractionPage", typeof(CurrentAttractionPage));
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
            _isBookmarked = (await _actionItemService.GetAction(1, 1)).Any(x => x.ItemId == _city.CityId);
            _isPinned = (await _actionItemService.GetAction(1, 2)).Any(x => x.ItemId == _city.CityId);
            _isViewed = (await _actionItemService.GetAction(1, 3)).Any(x => x.ItemId == _city.CityId);
            PinButton.Source = _isPinned ? "icon_filledPin.png" : "icon_emptyPin.png";
            EyeButton.Source = _isViewed ? "icon_filledEye.png" : "icon_emptyEye.png";
            if (_city.References.Count() > 0)
            {
                References.IsVisible = true;
                var attractionList = await _attractionService.Get();
                _attractions = new List<Attraction>();
                foreach (var item in _city.References)
                {
                    _attractions = _attractions.Append(attractionList.First(x => x.AttractionId == item));
                }

                BindableLayout.SetItemsSource(AttractionReferences, _attractions);
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
                await _actionItemService.DeleteAction(1, 1, (int)_city.CityId);
                _isBookmarked = false;
                DependencyService.Get<IToast>().Show("Место удалено из закладок");
            }
            else
            {
                await _actionItemService.AddAction(1, 1, (int)_city.CityId);
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
                await _actionItemService.DeleteAction(1, 2, (int)_city.CityId);
                DependencyService.Get<IToast>().Show("Пока не хочу посещать это место");
            }
            else
            {
                _isPinned = true;
                await _actionItemService.AddAction(1, 2, (int)_city.CityId);
                DependencyService.Get<IToast>().Show("Хочу посетить это место");
            }

            PinButton.Source = _isPinned ? "icon_filledPin.png" : "icon_emptyPin.png";  
        }

        private async void EyeButton_Tapped(object sender, EventArgs e)
        {
            if (_isViewed)
            {
                _isViewed = false;
                await _actionItemService.DeleteAction(1, 3, (int)_city.CityId);
                DependencyService.Get<IToast>().Show("Все-таки пока не увидел(а) это место :(");
            }
            else
            {
                _isViewed = true;
                await _actionItemService.AddAction(1, 3, (int)_city.CityId);
                DependencyService.Get<IToast>().Show("Я увидел(а) это место!");
            }

            EyeButton.Source = _isViewed ? "icon_filledEye.png" : "icon_emptyEye.png";
        }

        private async Task Reference_Tapped(Attraction sender)
        {
            await Shell.Current.GoToAsync($"/CurrentAttractionPage?id={(int)sender.AttractionId}");
        }
    }
}