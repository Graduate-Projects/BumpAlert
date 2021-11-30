using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace Bump.ViewModels
{
    public class ForgetPasswordViewModel : BaseViewModel
    {

        private Command sendEmailForgetPassword;
        public ICommand SendEmailForgetPassword => sendEmailForgetPassword ??= new Command(async () => await PerformSendEmailForgetPassword());

        public BLL.Models.ForgotPassword ForgetPass { get; set; }
        public ForgetPasswordViewModel()
        {
            ForgetPass = new BLL.Models.ForgotPassword();
        }
        private async Task PerformSendEmailForgetPassword()
        {
            if (string.IsNullOrWhiteSpace(ForgetPass.Email))
            {
                DisplayAlert("Required Feild", "Please, don't leave empty feild", "Okay");
            }
            else
            {
                try
                {
                    using (var loading = new Components.LoadingView())
                    {
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PostAsJsonAsync($"{BLL.Settings.Connections.GetServerAddress()}/api/account/forgotpassword", ForgetPass);
                            if (response.IsSuccessStatusCode)
                            {
                                await MaterialDialog.Instance.SnackbarAsync(Languages.MLResource.EmailSend, MaterialSnackbar.DurationLong, Constants.MaterialConfiguration.SnackbarConfiguration);
                            }
                            else
                            {
                                await MaterialDialog.Instance.SnackbarAsync(Languages.MLResource.ErrorOccure, MaterialSnackbar.DurationLong, Constants.MaterialConfiguration.SnackbarConfiguration);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    await MaterialDialog.Instance.SnackbarAsync(Languages.MLResource.ErrorOccure, MaterialSnackbar.DurationLong, Constants.MaterialConfiguration.SnackbarConfiguration);
                }
            }

        }
    }
}
