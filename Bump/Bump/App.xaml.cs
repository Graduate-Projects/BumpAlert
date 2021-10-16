using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI;
using localizer = Bump.Utils.LocalizationResourceManager;

namespace Bump
{
    public partial class App : Application
    {
        public static HubConnection _hubConnection;
        public App()
        {
            InitializeComponent();
            XF.Material.Forms.Material.Init(this);
            localizer.Initialization();
            StartPage().ConfigureAwait(false);
            _hubConnection = new HubConnectionBuilder().WithUrl($"{BLL.Settings.Connections.GetServerAddress()}/hub/location").WithAutomaticReconnect().Build();
            ConnectWithHub().ConfigureAwait(false);
        }

        private async Task StartPage()
        {
            Page page;
            if(await Xamarin.Essentials.SecureStorage.GetAsync("IsNotFirstRun") == null)
            {
                //this is first time run this application
                page = new Views.WorkThrough();
                await Xamarin.Essentials.SecureStorage.SetAsync("IsNotFirstRun", "true");
            }
            else
            {
                var token = await Xamarin.Essentials.SecureStorage.GetAsync("Auth.Key");
                if (!string.IsNullOrEmpty(token))
                {
                    AppStatic.AuthToken = token;
                }
                page = new Views.MainPageView.MainPage();
            }

            page.SetBinding(VisualElement.FlowDirectionProperty, new Binding(nameof(localizer.FlowDirection), source: localizer.Instance));
            MainPage = new NavigationPage(page);
        }
        public static async Task ConnectWithHub()
        {
            if(_hubConnection.State != HubConnectionState.Connected)
            await _hubConnection.StartAsync();
        }
        public static async Task StoptWithHub()
        {
            await _hubConnection.StopAsync();
        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            StoptWithHub().ConfigureAwait(false);
        }

        protected override void OnResume()
        {
            ConnectWithHub().ConfigureAwait(false);
        }
    }
}
