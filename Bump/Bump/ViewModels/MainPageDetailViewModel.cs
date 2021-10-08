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

namespace Bump.ViewModels
{
    public class MainPageDetailViewModel : BaseViewModel
    {
        public bool IsAuthentication => !string.IsNullOrEmpty(AppStatic.AuthToken);
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

                var location = await GetCurrentLocation();
                var DangerModel = new BLL.Models.Danger
                {
                    ID = Guid.NewGuid(),
                    DangerType = type,
                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                };
                var response = await httpClient.PostAsJsonAsync($"{BLL.Settings.Connections.GetServerAddress()}/api/marker/setdanger", DangerModel);
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
        CancellationTokenSource cts;
        private async Task<Xamarin.Essentials.Location> GetCurrentLocation() {

            Xamarin.Essentials.Location location = null;
            try
            {
                var request = new Xamarin.Essentials.GeolocationRequest(Xamarin.Essentials.GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                location = await Xamarin.Essentials.Geolocation.GetLocationAsync(request, cts.Token);
            }
            catch (Xamarin.Essentials.FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (Xamarin.Essentials.FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (Xamarin.Essentials.PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
            if (location == null)
                location = new Xamarin.Essentials.Location { Latitude = -1, Longitude = -1 };
            return location;
        }
    }
}
