using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Bump.ViewModels
{
    public class MainPageFlyoutViewModel : BaseViewModel
    {
        public bool IsAuthentication { get; set; }
        public bool IsAuthentication_2 { get; set; }
        public ICommand OpenAboutPageCommand { get; set; }
        public ICommand OpenSignInPageCommand { get; set; }
        public ICommand OpenContactUsPageCommand { get; set; }
        public ICommand OpenSuggestionPageCommand { get; set; }
        public ICommand InviteOtherPeopleCommand { get; set; }
        public ICommand OpenMainPageCommand { get; set; }
        public MainPageFlyoutViewModel()
        {
            OpenAboutPageCommand = new Command(OpenAboutPage);
            OpenSignInPageCommand = new Command(OpenSignInPage);
            OpenContactUsPageCommand = new Command(OpenContactUsPage);
            OpenSuggestionPageCommand = new Command(OpenSuggestionPage);
            InviteOtherPeopleCommand = new Command(InviteOtherPeople);
            IsAuthentication = !string.IsNullOrEmpty(AppStatic.AuthToken);
            IsAuthentication_2 = string.IsNullOrEmpty(AppStatic.AuthToken);
            OnPropertyChanged(nameof(OnPropertyChanged));
            OpenMainPageCommand = new Command(OpenMainPage);

          
        }
        private void OpenMainPage()
        {
            OpenPage(new Views.MainPageView.MainPage());

        }

       
        private void OpenAboutPage()
        {
            OpenPage(new Views.About());
        }
        private void OpenSignInPage()
        {
            OpenPage(new Views.SignIn());
        }
        private void OpenContactUsPage()
        {
            OpenPage(new Views.ContactUs());
        }
        private void OpenSuggestionPage()
        {
            OpenPage(new Views.Suggestions());

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
