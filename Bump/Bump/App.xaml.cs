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
            //MainPage = new Bump.Views.SignUp();
            MainPage = new Bump.Views.About();
            //MainPage = new Bump.Views.MainPageView.MainPage();
            //MainPage = new Views.SignIn();
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
