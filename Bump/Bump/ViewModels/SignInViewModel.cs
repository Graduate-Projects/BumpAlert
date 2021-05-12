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
    public class SignInViewModel : BaseViewModel
    {
        public BLL.Models.LoginRequest LoginRequest { get; set; }

        public ICommand OpenForgetPasswordPageCommand { get; set; }
        public ICommand SignInCommand { get; set; }
        public ICommand OpenSignUpPageCommand { get; set; }


        public SignInViewModel()
        {
            OpenForgetPasswordPageCommand = new Command(OpenForgetPasswordPage);
            SignInCommand = new Command(()=> SignIn().ConfigureAwait(false));
            OpenSignUpPageCommand = new Command(OpenSignUpPage);
            LoginRequest = new BLL.Models.LoginRequest();
        }

        private void OpenForgetPasswordPage()
        {
            OpenPage(new Views.ForgetPassword());

        }

        private async Task SignIn()
        {
            if (string.IsNullOrWhiteSpace(LoginRequest.Email) || string.IsNullOrWhiteSpace(LoginRequest.Password))
            {
                DisplayAlert("Required Feild", "Please, don't  leave empty feild", "Okay");
            }
            else
            {
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsJsonAsync($"{BLL.Settings.Connections.GetServerAddress()}/api/account/login", LoginRequest);
                        if (response.IsSuccessStatusCode)
                        {
                            OpenPage(new Views.MainPageContribute.Contribute());
                        }
                        else
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DisplayAlert("Can't SignIn", result, "Okay");
                        }
                    }
                }
                catch (Exception ex)
                {
                    DisplayAlert("Occurred an error", ex.ToString(), "Okay");
                }
            }
        }

        private void OpenSignUpPage()
        {

            OpenPage(new Views.SignUp());
        }
    }
}
