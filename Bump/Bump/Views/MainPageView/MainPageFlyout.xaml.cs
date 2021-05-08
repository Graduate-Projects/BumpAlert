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

namespace Bump.Views.MainPageView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageFlyout : ContentPage
    {
        public ListView ListView;

        public MainPageFlyout()
        {
            InitializeComponent();
        }

        private void OpenAboutPage(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PushAsync(new Views.AboutProgram());
        }

        private void OpenSignInPage(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PushAsync(new Views.SignIn());
        }

        private void OpenContactUsPage(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PushAsync(new Views.ContactUs());
        }

        private void OpenSuggestionPage(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PushAsync(new Views.Suggestions());

        }
    }
}