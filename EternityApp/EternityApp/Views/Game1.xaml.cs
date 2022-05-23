using EternityApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EternityApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Game1 : ContentPage
    {
        private readonly List<string> _images = new List<string>
        {
            "card1.jpg", "card1.jpg",
            "card2.jpg", "card2.jpg",
            "card3.jpg", "card3.jpg",
            "card4.jpg", "card4.jpg",
            "card5.jpg", "card5.jpg",
            "card6.jpg", "card6.jpg"
        };

        private readonly GameScoreService _gameScoreService;

        public Game1()
        {
            InitializeComponent();
            BusyLayout.IsVisible = true;
            MainLayout.IsVisible = false;
            LoadingWheel.IsRunning = true;
            _gameScoreService = new GameScoreService();
            AssignImagesToGrid();
            BusyLayout.IsVisible = false;
            MainLayout.IsVisible = true;
            LoadingWheel.IsRunning = false;
        }

        private void AssignImagesToGrid()
        {
            var rnd = new Random();
            int randInt;
            int row = 0, col = 0;
            for (var i = 0; i < 12; i++)
            {
                randInt = rnd.Next(0, _images.Count);
                var card = new StackLayout
                {
                    BackgroundColor = Color.FromHex("b385d9"),
                    Padding = 0,
                    WidthRequest = 100,
                    HeightRequest = 100,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Children =
                    {
                        new Image 
                        { 
                            Source = ImageSource.FromFile(_images[randInt]), 
                            Aspect = Aspect.AspectFit,
                            IsVisible = false 
                        }
                    }
                };

                var tapGesture = new TapGestureRecognizer();
                tapGesture.Tapped += (sender, e) => { Card_Tapped(sender, e); };
                card.GestureRecognizers.Add(tapGesture);
                GameGrid.Children.Add(card, col, row);
                _images.RemoveAt(randInt);
                col++;
                if (col == 3)
                {
                    row++;
                    col = 0;
                }
            }

            Device.StartTimer(TimeSpan.FromSeconds(1), OnStopwatchTick);
        }

        private int cardsLeft = 6;
        private StackLayout firstTapped = null;
        private StackLayout secondTapped = null;
        private bool isEndGame = false;
        private int minutes = 0;
        private int seconds = 0;

        private async void Card_Tapped(object sender, EventArgs e)
        {

            if (firstTapped != null && secondTapped != null)
                return;

            var card = sender as StackLayout;
            if (card == null)
                return;

            if (card.BackgroundColor == Color.Transparent)
                return;

            if (firstTapped == null)
            {
                firstTapped = card;
                card.BackgroundColor = Color.Transparent;
                card.Children.OfType<Image>().First().IsVisible = true;
                return;
            }

            secondTapped = card;
            card.BackgroundColor = Color.Transparent;
            card.Children.OfType<Image>().First().IsVisible = true;

            if (string.Equals(firstTapped.Children.OfType<Image>().First().Source.ToString(), secondTapped.Children.OfType<Image>().First().Source.ToString()))
            {
                firstTapped = null;
                secondTapped = null;
                cardsLeft -= 1;
                if (cardsLeft == 0)
                {
                    isEndGame = true;
                    TimeResult.Text = string.Format($"Время: {minutes:00}:{seconds:00}");
                    await _gameScoreService.Add(new Models.AddGameScore
                    {
                        GameId = 1,
                        UserId = Convert.ToInt32(await SecureStorage.GetAsync("ID")),
                        Score = string.Format($"{minutes:00}:{seconds:00}")

                    });

                    Models.GameScore gameScore = await _gameScoreService.Get(1);
                    BestScore.Text = $"Лучший результат:\n {gameScore.UserName} - {gameScore.Score}";
                    MainLayout.IsVisible = false;
                    GoBackLayout.IsVisible = true;
                }
            }
            else
            {
                Device.StartTimer(TimeSpan.FromSeconds(1), OnTimerTick);
            }
        }

        private bool OnTimerTick()
        {
            firstTapped.BackgroundColor = Color.FromHex("b385d9");
            secondTapped.BackgroundColor = Color.FromHex("b385d9");
            firstTapped.Children.OfType<Image>().First().IsVisible = false;
            secondTapped.Children.OfType<Image>().First().IsVisible = false;
            firstTapped = null;
            secondTapped = null;
            return false;
        }

        private bool OnStopwatchTick()
        {
            if (isEndGame)
            {
                return false;
            }

            seconds++;
            if(seconds == 60)
            {
                seconds = 0;
                minutes++;
            }

            Stopwatch.Text = string.Format($"Время: {minutes:00}:{seconds:00}");
            return true;
        }
    }
}