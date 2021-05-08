using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bump
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            XF.Material.Forms.Material.Init(this);
            //MainPage = new Bump.Views.MainPageContribute.Contribute();
            //MainPage = new Bump.Views.ContactUs();
            // MainPage = new Bump.Views.ForgetPassword();
            //MainPage = new Bump.Views.Suggestions();
            //MainPage = new Bump.Views.AboutUse();
            //MainPage = new Bump.Views.AboutProgram();
            //MainPage = new Bump.Views.SignUp();
            //MainPage = new Bump.Views.About();
            //MainPage = new Bump.Views.MainPageView.MainPage();
            //MainPage = new Views.SignIn();

            //MainPage = new NavigationPage(new Views.MainPageContribute.Contribute());
            MainPage = new NavigationPage(new Views.MainPageView.MainPage());

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
