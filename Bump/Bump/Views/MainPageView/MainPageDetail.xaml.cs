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
        private System.Timers.Timer timer;
        public Location CurrentPosition { get; set; }
        public MainPageDetail()
        {
            InitializeComponent();
            PrepareGoogleMap().ConfigureAwait(false);
            timer = new System.Timers.Timer();
            timer.AutoReset = true;
            timer.Interval = TimeSpan.FromSeconds(30).TotalSeconds;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(SetCurrentPosition);
            timer.Start();
            Console.ReadLine();
        }
        private async Task PrepareGoogleMap()
        {
            try
            {
                var LocationWhenInUsePermission = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                if (LocationWhenInUsePermission == PermissionStatus.Granted)
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
            catch (Exception)
            {

            }
            DangerButtons.IsVisible = !string.IsNullOrEmpty(AppStatic.AuthToken);
        }
        private async void SetCurrentPosition(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!RunTimer) return;
            try
            {
                var EndLocation = await Utils.Location.GetCurrentLocation(new CancellationTokenSource());
                if (Location.CalculateDistance(CurrentPosition, EndLocation, DistanceUnits.Kilometers) >= 0.500)
                {
                    await App.ConnectWithHub();
                    await App._hubConnection.InvokeAsync("CheckDangers", EndLocation.Latitude, EndLocation.Longitude);
                }
                CurrentPosition = EndLocation;
            }
            catch (Exception)
            {

            }
        }

    }
}