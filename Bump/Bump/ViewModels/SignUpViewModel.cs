using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Bump.ViewModels
{

    public class SignUpViewModel
    {
        public BLL.Models.User User { get; set; }
        public string PasswordConfirm { get; set; }

        public ICommand OpenSignInPageCommand { get; set; }
        public ICommand OpenSignUpPageCommand { get; set; }

        public SignUpViewModel()
        {
            OpenSignInPageCommand = new Command(OpenSignInPage);
            OpenSignUpPageCommand = new Command(() => OpenSignUpPage().ConfigureAwait(false));
        }
        private async Task OpenSignUpPage()
        {
            if (IsValidUser(User))
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsJsonAsync($"{BLL.Settings.Connections.GetServerAddress()}/api/account/register", User);
                    if (response.IsSuccessStatusCode)
                    {
                        Application.Current.MainPage.Navigation.PushAsync(new Views.MainPageContribute.Contribute());
                    }
                    else
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        Application.Current.MainPage.DisplayAlert("Can't Login", result, "Okay");
                    }
                }
            }
        }
        private bool IsValidUser(BLL.Models.User user)
        {
            return !string.IsNullOrWhiteSpace(user.FirstName) &&
                    !string.IsNullOrWhiteSpace(user.LastName) &&
                    !string.IsNullOrWhiteSpace(user.PhoneNumber) &&
                    user.Password == PasswordConfirm;
        }
        private void OpenSignInPage()
        {
            App.Current.MainPage.Navigation.PopAsync();
        }
    }
}
