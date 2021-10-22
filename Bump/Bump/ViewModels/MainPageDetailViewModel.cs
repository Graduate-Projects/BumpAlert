using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;
using Microsoft.AspNetCore.SignalR.Client;

namespace Bump.ViewModels
{
    public class MainPageDetailViewModel : BaseViewModel
    {
        public ICommand ReportDangerCommand { get; set; }
        public MainPageDetailViewModel()
        {
            ReportDangerCommand = new Command<BLL.Enums.DangerType>((type)=> ReportDanger(type).ConfigureAwait(false));

        }

        private async Task ReportDanger(BLL.Enums.DangerType type)
        {
            try
            {
                using var httpClient = new HttpClient();

                var location = await Utils.Location.GetCurrentLocation(new CancellationTokenSource());
                var DangerModel = new BLL.Models.Danger
                {
                    ID = Guid.NewGuid(),
                    DangerType = type,
                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                    CreateBy = AppStatic.Username
                };
                var response = await httpClient.PostAsJsonAsync($"{BLL.Settings.Connections.GetServerAddress()}/api/marker", DangerModel);
                if (response.IsSuccessStatusCode)
                {
                    await MaterialDialog.Instance.SnackbarAsync(Languages.MLResource.SuccessedSetDanger, MaterialSnackbar.DurationLong);
                }
                else
                {
                    await MaterialDialog.Instance.SnackbarAsync(Languages.MLResource.FailedSetDanger, MaterialSnackbar.DurationLong);
                }
            }
            catch (Exception)
            {
                await MaterialDialog.Instance.SnackbarAsync(Languages.MLResource.FailedSetDanger, MaterialSnackbar.DurationLong);
            }
        }
    }
}
