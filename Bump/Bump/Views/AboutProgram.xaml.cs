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
    public partial class AboutProgram : ContentPage
    {
        public AboutProgram()
        {
            InitializeComponent();
        }

        private void OpenMainPage(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PushAsync(new Views.MainPageView.MainPage());

        }

        private void OpenAboutPage(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PushAsync(new Views.AboutUse());

        }
    }
}