using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bump.Views.MainPageContribute
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContributeFlyout : ContentPage
    {
        public ListView ListView;

        public ContributeFlyout()
        {
            InitializeComponent();

            
        }

        private void OpenAboutPage(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PushAsync(new Views.About());
        }   

        private void OpenContactUsPage(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PushAsync(new Views.ContactUs());
        }

        private void OpenSuggestionPage(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PushAsync(new Views.Suggestions());

        }

        private void OpenMainPage(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PushAsync(new Views.MainPageView.MainPage());

        }
    }
}