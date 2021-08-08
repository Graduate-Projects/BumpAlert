using Bump.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Auth;
using Xamarin.Forms;

namespace Bump.ViewModels
{
    public class SignInViewModel : BaseViewModel
    {
        public BLL.Models.LoginRequest LoginRequest { get; set; }

        public ICommand SignInCommand { get; set; }
        public ICommand LoginUsingFacebook { get; set; }
        public ICommand LoginUsingGoogle { get; set; }
        public ICommand OpenForgetPasswordPageCommand { get; set; }
        public ICommand OpenSignUpPageCommand { get; set; }

        private OAuth2Authenticator _authenticator;
        public SignInViewModel()
        {
            OpenForgetPasswordPageCommand = new Command(OpenForgetPasswordPage);
            SignInCommand = new Command(async () => await SignIn());
            OpenSignUpPageCommand = new Command(OpenSignUpPage);
            LoginRequest = new BLL.Models.LoginRequest();
            LoginUsingFacebook = new Command(async () => await PreformLoginUsingFacebook());
            LoginUsingGoogle = new Command(async () => await PreformLoginUsingGoogle());
        }

        private async Task PreformLoginUsingGoogle()
        {
            _authenticator = new OAuth2Authenticator(
                BLL.Constants.Google.ClientId, 
                BLL.Constants.Google.ClientSecret, 
                scope: "https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile",
                authorizeUrl: new Uri("https://accounts.google.com/o/oauth2/auth"),
                redirectUrl: new Uri($"com.googleusercontent.apps.{BLL.Constants.Google.ClientId}:/oauth2redirect"),
                accessTokenUrl: new Uri("https://www.googleapis.com/oauth2/v4/token"),
                isUsingNativeUI: true);

            _authenticator.Completed += Authenticator_Google_Completed;
            _authenticator.Error += Authenticator_Error;
            AuthenticationState.Authenticator = _authenticator;
            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(_authenticator);
        }
        private async void Authenticator_Google_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            try
            {
                _authenticator.Completed -= Authenticator_Google_Completed;
                if (e.IsAuthenticated)
                {
                    var userToken = await GetGoogleProfile(e.Account);
                    await ExternalLogin(userToken, BLL.Enums.AuthProvider.Google);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
        public async Task<BLL.Models.Identity.SocialToken> GetGoogleProfile(Account account)
        {
            var request = new OAuth2Request("GET", new Uri("https://www.googleapis.com/oauth2/v2/userinfo"), null, account);
            var response = await request.GetResponseAsync();

            if(response != null)
            {
                string userJson = await response.GetResponseTextAsync();
                var user = System.Text.Json.JsonSerializer.Deserialize<BLL.Models.Identity.SocialToken>(userJson);
                user.TokenId = account.Properties["id_token"];
                return user;
            }
            throw new Exception("Could not get user");
        }
        private async Task PreformLoginUsingFacebook()
        {
            _authenticator = new OAuth2Authenticator(BLL.Constants.Facebook.AppId, "email",
            authorizeUrl: new Uri("https://www.facebook.com/dialog/oauth"),
            redirectUrl: new Uri("https://hucoinapi.azurewebsites.net/page/privacypolicy"));

            _authenticator.Completed += Authenticator_Facebook_Completed;
            _authenticator.Error += Authenticator_Error;
            AuthenticationState.Authenticator = _authenticator;
            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(_authenticator);
        }
        private async void Authenticator_Facebook_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            _authenticator.Completed -= Authenticator_Facebook_Completed;
            if (e.IsAuthenticated)
            {
                var user = await GetFacebookProfile(e.Account.Properties["access_token"]);
                var userToken = new BLL.Models.Identity.SocialToken
                {
                    TokenId = e.Account.Properties["access_token"],
                    Email = user.Email,
                    FamilyName = user.FirstName,
                GivenName = user.LastName,
            };
                await ExternalLogin(userToken, BLL.Enums.AuthProvider.Facebook);
            }
        }
        public async Task<BLL.Models.Identity.Facebook.User> GetFacebookProfile(string accessToken)
        {
            using var httpClient = new HttpClient();

            var json = await httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,first_name,last_name&access_token={accessToken}");

            return System.Text.Json.JsonSerializer.Deserialize<BLL.Models.Identity.Facebook.User>(json);
        }

        private void Authenticator_Error(object sender, AuthenticatorErrorEventArgs e)
        {
            _authenticator.Error -= Authenticator_Error;
            Debug.WriteLine(e.Message);
        }
        private async Task ExternalLogin(BLL.Models.Identity.SocialToken userToken, BLL.Enums.AuthProvider authProvider)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsJsonAsync($"{BLL.Settings.Connections.GetServerAddress()}/api/account/{authProvider}", userToken);
            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                OpenPage(new Views.MainPageContribute.Contribute());
            }
            else
            {
                var result = await response.Content.ReadAsStringAsync();
                DisplayAlert("Can't SignIn", result, "Okay");
            }

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
                            var token = await response.Content.ReadAsStringAsync();
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
