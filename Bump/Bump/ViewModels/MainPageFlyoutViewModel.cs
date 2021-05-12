using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Bump.ViewModels
{
    public class MainPageFlyoutViewModel : BaseViewModel
    {
        public ICommand OpenAboutPageCommand { get; set; }
        public ICommand OpenSignInPageCommand { get; set; }
        public ICommand OpenContactUsPageCommand { get; set; }
        public ICommand OpenSuggestionPageCommand { get; set; }
        public ICommand InviteOtherPeopleCommand { get; set; }
        public MainPageFlyoutViewModel()
        {
            OpenAboutPageCommand = new Command(OpenAboutPage);
            OpenSignInPageCommand = new Command(OpenSignInPage);
            OpenContactUsPageCommand = new Command(OpenContactUsPage);
            OpenSuggestionPageCommand = new Command(OpenSuggestionPage);
            InviteOtherPeopleCommand = new Command(InviteOtherPeople);
        }
        private void OpenAboutPage()
        {
            App.Current.MainPage.Navigation.PushAsync(new Views.About());
        }
        private void OpenSignInPage()
        {
            App.Current.MainPage.Navigation.PushAsync(new Views.SignIn());
        }
        private void OpenContactUsPage()
        {
            App.Current.MainPage.Navigation.PushAsync(new Views.ContactUs());
        }
        private void OpenSuggestionPage()
        {
            App.Current.MainPage.Navigation.PushAsync(new Views.Suggestions());

        }
        private void InviteOtherPeople()
        {
            Xamarin.Essentials.Share.RequestAsync(new Xamarin.Essentials.ShareTextRequest
            {
                Title = "Hello My Friend",
                Subject = "Download Bump Application",
                Text = "I invite you to download bump application",
                Uri = "https://play.google.com/store/apps/details?id=com.just.bump"
            });
        }
    }
}
