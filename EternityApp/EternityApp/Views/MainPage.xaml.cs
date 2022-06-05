using EternityApp.Models;
using EternityApp.Services;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EternityApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private readonly GameScoreService _gameScoreService;
        private GameScore game1Score;
        private GameScore game2Score;
        public MainPage()
        {
            InitializeComponent();
            _gameScoreService = new GameScoreService();
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
            try
            {
                Game1Result.IsVisible = true;
                Game1NoResult.IsVisible = false;
                game1Score = await _gameScoreService.Get(1);
                Game1UserName.Text = game1Score.UserName;
                Game1UserScore.Text = $"Время: {game1Score.Score}";
                if (game1Score.Image == null)
                {
                    Game1UserImage.Source = "icon_no_avatar.jpg";
                }
                else
                {
                    Game1UserImage.Source = $"{AppSettings.Url}images/Users/{game1Score.UserId}/{game1Score.Image}";
                }
            }
            catch
            {
                Game1Result.IsVisible = false;
                Game1NoResult.IsVisible = true;
            }

            try
            {
                Game2Result.IsVisible = true;
                Game2NoResult.IsVisible = false;
                game2Score = await _gameScoreService.Get(2);
                Game2UserName.Text = game2Score.UserName;
                Game2UserScore.Text = $"Правильных ответов: {game2Score.Score}";
                if (game2Score.Image == null)
                {
                    Game2UserImage.Source = "icon_no_avatar.jpg";
                }
                else
                {
                    Game2UserImage.Source = $"{AppSettings.Url}images/Users/{game2Score.UserId}/{game2Score.Image}";
                }
            }
            catch
            {
                Game2Result.IsVisible = false;
                Game2NoResult.IsVisible = true;
            }
        }

        private async void RefreshView_Refreshing(object sender, System.EventArgs e)
        {
            await LoadData();
            RefreshView.IsRefreshing = false;
        }
    }
}