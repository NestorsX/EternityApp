using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EternityApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SavedPage : ContentPage
    {
        public SavedPage()
        {
            InitializeComponent();
            Routing.RegisterRoute("/CityBookmarksPage", typeof(CityBookmarksPage));
            Routing.RegisterRoute("/AttractionBookmarksPage", typeof(AttractionBookmarksPage));
        }

        private async void CityBookmarks_Tapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"/CityBookmarksPage");
        }

        private async void AttractionBookmarks_Tapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"/AttractionBookmarksPage");
        }
    }
}