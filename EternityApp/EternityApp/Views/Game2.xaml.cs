using EternityApp.Models;
using EternityApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EternityApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Game2 : ContentPage
    {
        private IEnumerable<TestQuestion> _testQuestions;
        private readonly TestQuestionService _testQuestionService;
        private readonly GameScoreService _gameScoreService;
        private readonly Random _random;
        private int _rightAnswers;
        private int _questionNumber;

        public Game2()
        {
            InitializeComponent();
            _testQuestionService = new TestQuestionService();
            _gameScoreService = new GameScoreService();
            _random = new Random();
            _rightAnswers = 0;
            _questionNumber = 1;
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
            _testQuestions = await _testQuestionService.Get();
            foreach (var item in _testQuestions)
            {
                if (item.Image != null)
                {
                    item.Image = $"{AppSettings.Url}images/TestQuestions/{item.TestQuestionId}/{item.Image}";
                }
            }

            PrepareQuestion();
        }

        private void PrepareQuestion()
        {
            ImageBox.IsVisible = true;
            var currentQuestion = _testQuestions.ElementAt(_questionNumber - 1);
            QuestionNumber.Text = $"Вопрос {_questionNumber}/10";
            QuestionLabel.Text = currentQuestion.Question;
            if (currentQuestion.Image == null)
            {
                ImageBox.IsVisible = false;
            }

            ImageBox.Source = currentQuestion.Image;
            List<Button> buttons = new List<Button>
            {
                AnswerButton1, AnswerButton2, AnswerButton3, AnswerButton4
            };

            buttons = buttons.OrderBy(x => _random.Next()).ToList();
            buttons[0].Text = currentQuestion.RightAnswer;
            buttons[1].Text = currentQuestion.WrongAnswer1;
            buttons[2].Text = currentQuestion.WrongAnswer2;
            buttons[3].Text = currentQuestion.WrongAnswer3;
        }

        private async void AnswerButton_Clicked(object sender, EventArgs e)
        {
            if ((sender as Button).Text.Equals(_testQuestions.ElementAt(_questionNumber - 1).RightAnswer))
            {
                ++_rightAnswers;
            }

            ++_questionNumber;
            if (_questionNumber > 10)
            {
                RightAnswerResult.Text = $"Правильных ответов: {_rightAnswers}";
                await _gameScoreService.Add(new AddGameScore
                {
                    GameId = 2,
                    UserId = Convert.ToInt32(await SecureStorage.GetAsync("ID")),
                    Score = _rightAnswers.ToString(),
                });

                GameScore gameScore = await _gameScoreService.Get(2);
                BestScore.Text = $"Лучший результат:\n {gameScore.UserName} - правильных ответов: {gameScore.Score}";
                MainLayout.IsVisible = false;
                GoBackLayout.IsVisible = true;
                return;
            }

            PrepareQuestion();
        }
    }
}