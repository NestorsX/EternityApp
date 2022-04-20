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
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            InfoLabel.Text = "Дипломный проект\n" +
                "выпускницы группы 32о 2022 года\n" +
                "Панфиловой Дианы Дмитриевны";
        }
    }
}