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
    public partial class SignUp : ContentPage
    {
        public SignUp()
        {
            InitializeComponent();
        }

        private void OpenSginInPage(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PushAsync(new Views.SignIn());

        }

        private void OpenContributePage(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PushAsync(new Views.MainPageContribute.Contribute());

        }
    }
}