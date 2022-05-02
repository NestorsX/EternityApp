using EternityApp.Models;
using EternityApp.Services;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EternityApp.Views
{
    [QueryProperty(nameof(Id), "id")]
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class CurrentAttractionPage : ContentPage
    {
        private readonly AttractionService _attractionService;
        private readonly ImageService _imageService;
        private Attraction _attraction;
        private int _id;

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
            BusyLayout.IsVisible = false;
            MainLayout.IsVisible = true;
            LoadingWheel.IsRunning = false;
        }
    }
}