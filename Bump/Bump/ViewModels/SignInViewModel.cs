using System;
using System.Collections.Generic;
using System.Text;

namespace Bump.ViewModels
{
    public class SignInViewModel
    {
        public Models.LoginRequest LoginRequest { get; set; }
        public SignInViewModel()
        {

        }

        private void OpenForgetPasswordPage()
        {
            App.Current.MainPage.Navigation.PushAsync(new Views.ForgetPassword());

        }

        private void OpenContributePage()
        {
            App.Current.MainPage.Navigation.PushAsync(new Views.MainPageContribute.Contribute());

        }

        private void OpenSginUpPage()
        {

            App.Current.MainPage.Navigation.PushAsync(new Views.SignUp());
        }
    }
}
