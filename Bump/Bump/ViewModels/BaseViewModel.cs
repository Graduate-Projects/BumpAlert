using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Bump.ViewModels
{
    public class BaseViewModel
    {
        public void OpenPageAsMainPage(Page page)
        {
            App.Current.MainPage = new NavigationPage(page);
        }
        public void OpenPage(Page page)
        {
            App.Current.MainPage.Navigation.PushAsync(page);
        }
        public void BackPage()
        {
            App.Current.MainPage.Navigation.PopAsync();
        }
        public void DisplayAlert(string title, string message, string cancelButton)
        {
            App.Current.MainPage.DisplayAlert(title, message, cancelButton);
        }
    }
}
