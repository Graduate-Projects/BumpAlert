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
    public partial class SignIn : ContentPage
    {
        public SignIn()
        {
            InitializeComponent();
        }

        private void OpenForgetPasswordPage(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PushAsync(new Views.ForgetPassword());

        }

        private void OpenContributePage(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PushAsync(new Views.MainPageContribute.Contribute());

        }

        private void OpenSginUpPage(object sender, EventArgs e)
        {

            App.Current.MainPage.Navigation.PushAsync(new Views.SignUp());
        }
    }
}