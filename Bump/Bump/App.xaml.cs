﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bump
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new Bump.Views.SignIn();
            //MainPage = new Bump.Views.MainPageView.MainPage();
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
