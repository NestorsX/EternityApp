using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EternityApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            Routing.RegisterRoute("//RegisterPage", typeof(RegisterPage));
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            LoadingWheel.IsRunning = true;
            await Shell.Current.GoToAsync("//MainPage");
            LoadingWheel.IsRunning = false;
        }

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(RegisterPage)}");
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }
    }
}