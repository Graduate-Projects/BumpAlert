using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Bump.ViewModels
{
    public class SignInViewModel
    {
        public BLL.Models.LoginRequest LoginRequest { get; set; }

        public ICommand OpenForgetPasswordPageCommand { get; set; }
        public ICommand OpenContributePageCommand { get; set; }
        public ICommand OpenSignUpPageCommand { get; set; }


        public SignInViewModel()
        {
            OpenForgetPasswordPageCommand = new Command(OpenForgetPasswordPage);
            OpenContributePageCommand = new Command(OpenContributePage);
            OpenSignUpPageCommand = new Command(OpenSignUpPage);

        }

        private void OpenForgetPasswordPage()
        {
            App.Current.MainPage.Navigation.PushAsync(new Views.ForgetPassword());

        }

        private void OpenContributePage()
        {
            App.Current.MainPage.Navigation.PushAsync(new Views.MainPageContribute.Contribute());

        }

        private void OpenSignUpPage()
        {

            App.Current.MainPage.Navigation.PushAsync(new Views.SignUp());
        }
    }
}
