using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bump.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutUse : ContentPage
    {
        public AboutUse()
        {
            InitializeComponent();
        }

        private void OpenMainPage(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PushAsync(new Views.MainPageView.MainPage());

        }

        private void OpenAbouTeamtPage(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PushAsync(new Views.About());

        }
    }
}