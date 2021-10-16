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

namespace Bump.Views.MainPageView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageDetail : ContentPage
    {
        public bool RunTimer = false;
        public Location CurrentPosition { get; set; }
        public MainPageDetail()
        {
            InitializeComponent();
            Position = new Location[2];
            PrepareGoogleMap().ConfigureAwait(false);
        }
        private async Task PrepareGoogleMap()
        {
            try
            {
                var LocationWhenInUsePermission = await Xamarin.Essentials.Permissions.RequestAsync<Xamarin.Essentials.Permissions.LocationWhenInUse>();
                if (LocationWhenInUsePermission == Xamarin.Essentials.PermissionStatus.Granted)
                {
                    RunTimer = true;
                    var location = await Utils.Location.GetCurrentLocation(new CancellationTokenSource());
                    var GoogleMap = new Xamarin.Forms.GoogleMaps.Map()
                    {
                        MyLocationEnabled = true,
                    };
                    GoogleMap.UiSettings.MyLocationButtonEnabled = true;
                    GoogleMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromMiles(1)));
                    MapGoogle.Children.Add(GoogleMap);
                }
            }
            catch (Exception ex)
            {

            }
            DangerButtons.IsVisible = !string.IsNullOrEmpty(AppStatic.AuthToken);
        }
        private async Task SetCurrentPosition()
        {
            if (!RunTimer) return;
            try
            {
                var EndLocation = await Utils.Location.GetCurrentLocation(new CancellationTokenSource());
                if (Location.CalculateDistance(CurrentPosition, EndLocation, DistanceUnits.Kilometers) >= 0.500)
                {
                    if (App._hubConnection.State == HubConnectionState.Connected)
                    {
                        await App._hubConnection.InvokeAsync("CheckDangers", EndLocation.Latitude, EndLocation.Longitude);
                    }
                    else
                    {
                        await App.ConnectWithHub();
                    }
                }
                CurrentPosition = EndLocation;
            }
            catch (Exception)
            {

            }
        }

    }
}