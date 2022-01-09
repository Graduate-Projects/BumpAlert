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
using Microsoft.AppCenter;
using Xamarin.Essentials;
using Microsoft.AppCenter.Crashes;

namespace Bump.ViewModels
{
    public class MainPageDetailViewModel : BaseViewModel
    {
        public ICommand ReportDangerCommand { get; set; }
        public bool IsAllowedPermission = false;
        private System.Timers.Timer timer;
        public double IntervalCheck { get; set; }
        public Location LastPosition { get; set; }
        public MainPageDetailViewModel()
        {
            AppCenter.SetUserId(AppStatic.Username);
            ReportDangerCommand = new Command<BLL.Enums.DangerType>((type) => ReportDanger(type).ConfigureAwait(false));
            IntervalCheck = TimeSpan.FromSeconds(30).TotalSeconds;

            timer = new System.Timers.Timer();
            timer.AutoReset = true;
            timer.Interval = IntervalCheck * 1000;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(SetCurrentPosition);
            timer.Start();
        }
        private async void SetCurrentPosition(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!IsAllowedPermission) return;
            try
            {
                LastPosition = await Utils.Location.GetCurrentLocation(new CancellationTokenSource());
            }
            catch (Exception)
            {

            }
        }
        private async Task ReportDanger(BLL.Enums.DangerType type)
        {
            try
            {
                using (var loading = new Components.LoadingView())
                {

                    using var httpClient = new HttpClient();

                    var location = await Utils.Location.GetCurrentLocation(new CancellationTokenSource());
                    var SignWay = LastPosition != null ? Math.Sign((location.Longitude - LastPosition.Longitude) + (location.Latitude - LastPosition.Latitude)) : -1;
                    var DangerModel = new BLL.Models.Danger
                    {
                        ID = Guid.NewGuid(),
                        DangerType = type,
                        Latitude = location.Latitude,
                        Longitude = location.Longitude,
                        CreateBy = AppStatic.Username,
                        ForwordBackword = SignWay
                    };
                    var response = await httpClient.PostAsJsonAsync($"{BLL.Settings.Connections.GetServerAddress()}/api/marker/setdanger", DangerModel);
                    if (response.IsSuccessStatusCode)
                    {
                        await MaterialDialog.Instance.SnackbarAsync(Languages.MLResource.SuccessedSetDanger, MaterialSnackbar.DurationLong, Constants.MaterialConfiguration.SnackbarConfiguration);
                    }
                    else
                    {
                        await MaterialDialog.Instance.SnackbarAsync(Languages.MLResource.FailedSetDanger, MaterialSnackbar.DurationLong, Constants.MaterialConfiguration.SnackbarConfiguration);
                        var content = await response.Content.ReadAsStringAsync();
                        var properties = new Dictionary<string, string> { { "Email", AppStatic.Username }, { "Erorr_Message", content } };
                        Crashes.TrackError(new Exception(content), properties);

                    }
                }
            }
            catch (Exception exception)
            {
                await MaterialDialog.Instance.SnackbarAsync(Languages.MLResource.FailedSetDanger, MaterialSnackbar.DurationLong, Constants.MaterialConfiguration.SnackbarConfiguration);

                var properties = new Dictionary<string, string> {
                    { "Email", AppStatic.Username }
                };
                Crashes.TrackError(exception, properties);
            }
        }
    }
}
