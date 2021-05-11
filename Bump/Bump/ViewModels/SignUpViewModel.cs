using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Bump.ViewModels
{

    public class SignUpViewModel
    {
        public Models.User User { get; set; }
        public ICommand OpenContributePageCommand { get; set; }
        public ICommand OpenSignInPageCommand { get; set; }

        public SignUpViewModel()
        {
            OpenContributePageCommand = new Command(OpenContributePage);
            OpenSignInPageCommand = new Command(OpenSignInPage);
        }


        private void OpenSignInPage()
        {
            App.Current.MainPage.Navigation.PopAsync();
        }

        private void OpenContributePage()
        {
            App.Current.MainPage.Navigation.PushAsync(new Views.MainPageContribute.Contribute());
        }
    }
}
