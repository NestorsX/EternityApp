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
    public partial class GamesPage : ContentPage
    {
        public GamesPage()
        {
            InitializeComponent();
            Routing.RegisterRoute("//Game1", typeof(Game1));
        }

        private async void Game1_Tapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(Game1)}");
        }
    }
}