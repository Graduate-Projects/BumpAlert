﻿using Microsoft.AppCenter;
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
        public double IntervalCheck { get; set; }
        public Location LastPosition { get; set; }
        public MainPageDetail()
        {
            InitializeComponent();
            PrepareGoogleMap().ConfigureAwait(false);
            IntervalCheck = TimeSpan.FromSeconds(30).TotalSeconds;

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
                    LastPosition = await Utils.Location.GetCurrentLocation(new CancellationTokenSource());
                    var GoogleMap = new Xamarin.Forms.GoogleMaps.Map()
                    {
                        MyLocationEnabled = true,
                    };
                    GoogleMap.UiSettings.MyLocationButtonEnabled = true;
                    GoogleMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(LastPosition.Latitude, LastPosition.Longitude), Distance.FromMiles(1)));
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
                var CurrentLocation = await Utils.Location.GetCurrentLocation(new CancellationTokenSource());
                var Distance = Location.CalculateDistance(LastPosition, CurrentLocation, DistanceUnits.Kilometers) * 1000; //calculate distance between last position and current poistion in meters
                if (Distance >= 35)
                {
                    LastPosition = CurrentLocation;
                    await App.ConnectWithHub();
                    if (App._hubConnection.State == HubConnectionState.Connected)
                    {
                        await App._hubConnection.InvokeAsync("CheckDangers", CurrentLocation.Latitude, CurrentLocation.Longitude, Distance).ConfigureAwait(false);
                    }
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
    }
}