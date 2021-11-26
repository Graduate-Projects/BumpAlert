using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace Bump.Views.MainPageView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageDetail : ContentPage
    {
        public bool IsAllowedPermission = false;
        private System.Timers.Timer timer;
        public Location CurrentPosition { get; set; }
        public double IntervalCheck { get; set; }
        public MainPageDetail()
        {
            InitializeComponent();
            PrepareGoogleMap().ConfigureAwait(false);
            IntervalCheck = TimeSpan.FromMinutes(5).TotalSeconds;

            timer = new System.Timers.Timer();
            timer.AutoReset = true;
            timer.Interval = IntervalCheck * 1000;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(SetCurrentPosition);
            timer.Start();
        }

        private async Task PrepareGoogleMap()
        {
            try
            {
                var LocationWhenInUsePermission = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                if (LocationWhenInUsePermission == PermissionStatus.Granted)
                {
                    IsAllowedPermission = true;
                    CurrentPosition = await Utils.Location.GetCurrentLocation(new CancellationTokenSource());
                    var GoogleMap = new Xamarin.Forms.GoogleMaps.Map()
                    {
                        MyLocationEnabled = true,
                    };
                    GoogleMap.UiSettings.MyLocationButtonEnabled = true;
                    GoogleMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(CurrentPosition.Latitude, CurrentPosition.Longitude), Distance.FromMiles(1)));
                    MapGoogle.Children.Add(GoogleMap);
                }
            }
            catch (Exception)
            {

            }
            DangerButtons.IsVisible = !string.IsNullOrEmpty(AppStatic.AuthToken);
        }
        private async void SetCurrentPosition(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!IsAllowedPermission) return;
            try
            {
                var EndLocation = await Utils.Location.GetCurrentLocation(new CancellationTokenSource());
                var Distance = Location.CalculateDistance(CurrentPosition, EndLocation, DistanceUnits.Kilometers);
                CurrentPosition = EndLocation;
                var Speed = Distance / IntervalCheck;

                if (Distance >= 0.005) // 5 metear
                {
                    await App.ConnectWithHub();
                    await App._hubConnection.InvokeAsync("CheckDangers", EndLocation.Latitude, EndLocation.Longitude).ConfigureAwait(false);
                    Analytics.TrackEvent("Check Current Position", new Dictionary<string, string> {
                        { "User", AppStatic.Username},
                        { "Speed", Speed.ToString()},
                        { "Location", EndLocation.ToString()},
                        { "Distance From Old Distance", Distance.ToString() }
                    });
                }
                else
                {
                    Analytics.TrackEvent("Check Current Position - Not Work", new Dictionary<string, string> {
                        { "User", AppStatic.Username},
                        { "Speed", Speed.ToString()},
                        { "Location", EndLocation.ToString()},
                        { "Distance From Old Distance", Distance.ToString() }
                    });
                }
            }
            catch (Exception exception)
            {
                var properties = new Dictionary<string, string> {
                    { "Email", AppStatic.Username }
                };
                Crashes.TrackError(exception, properties);
            }
        }

        private async void ReconnectSignalR(object sender, EventArgs e)
        {
            try
            {
                this.SignalRButtons.IsVisible = false;
                await App.ConnectWithHub();

                this.SignalRButtons.IsVisible = App._hubConnection.State != HubConnectionState.Connected;
            }
            catch (Exception exception)
            {
                await MaterialDialog.Instance.SnackbarAsync(exception.Message, 2750).ConfigureAwait(false);
                Crashes.TrackError(exception);
                this.SignalRButtons.IsVisible = true;
            }
        }
    }
}