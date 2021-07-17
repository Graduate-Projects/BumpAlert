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
    public class SignUpViewModel : BaseViewModel
    {
        public BLL.Models.User User { get; set; }
        public string PasswordConfirm { get; set; }

        public ICommand OpenSignInPageCommand { get; set; }
        public ICommand OpenSignUpPageCommand { get; set; }

        public SignUpViewModel()
        {
            User = new BLL.Models.User();
            OpenSignInPageCommand = new Command(OpenSignInPage);
            OpenSignUpPageCommand = new Command(() => OpenSignUpPage().ConfigureAwait(false));
        }
        private async Task OpenSignUpPage()
        {
            if (IsValidUser(User))
            {
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsJsonAsync($"{BLL.Settings.Connections.GetServerAddress()}/api/account/register", User);
                        if (response.IsSuccessStatusCode)
                        {
                            var token = await response.Content.ReadAsStringAsync();
                            OpenPage(new Views.MainPageContribute.Contribute());
                        }
                        else
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DisplayAlert("Can't SignUp", result, "Okay");
                        }
                    }
                }
                catch (Exception ex)
                {
                    DisplayAlert("Occurred an error", ex.ToString(), "Okay"); ;
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
            BackPage();
        }
    }
}
