using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using localizer = Bump.Utils.LocalizationResourceManager;
namespace Bump.ViewModels
{
    public class BaseViewModel
    {
        public ICommand ChangeLanguageCommand { get; set; }
        public BaseViewModel()
        {
            ChangeLanguageCommand = new Command<string>(culture=> ChangeLanguage(culture));
        }
        private void ChangeLanguage(string culture)
        {
            localizer.Instance.SetCulture(new System.Globalization.CultureInfo(culture));
        }
        public void OpenPageAsMainPage(Page page)
        {
            page.SetBinding(VisualElement.FlowDirectionProperty, new Binding(nameof(localizer.FlowDirection), source: localizer.Instance));
            App.Current.MainPage = new NavigationPage(page);
        }
        public void OpenPage(Page page)
        {
            page.SetBinding(VisualElement.FlowDirectionProperty, new Binding(nameof(localizer.FlowDirection), source: localizer.Instance));
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
