using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI;
using XF.Material.Forms.UI.Dialogs;
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
                var username = await Xamarin.Essentials.SecureStorage.GetAsync("Auth.Username");

                AppStatic.AuthToken = token;
                AppStatic.Username = username;

                page = new Views.MainPageView.MainPage();
            }

            page.SetBinding(VisualElement.FlowDirectionProperty, new Binding(nameof(localizer.FlowDirection), source: localizer.Instance));
            MainPage = new NavigationPage(page);
        }
        public static async Task ConnectWithHub()
        {
            if (_hubConnection.State != HubConnectionState.Connected)
            {
                _hubConnection.On<BLL.Enums.DangerType>("DetectDanger", async(DangerType) => {
                    string Message = string.Empty;
                    Message = DangerType switch
                    {
                        BLL.Enums.DangerType.RADAR => Languages.MLResource.SpeedAlert,
                        BLL.Enums.DangerType.BUMP => Languages.MLResource.BumpAlert,
                        BLL.Enums.DangerType.PIT => Languages.MLResource.PitAlert,
                        _ => "BeCarful",
                    };
                    await MaterialDialog.Instance.SnackbarAsync(Message, MaterialSnackbar.DurationLong);
                    await SpeakNow(Message).ConfigureAwait(false);
                });
                await _hubConnection.StartAsync();
            }
        }
        public static async Task StoptWithHub()
        {
            await _hubConnection.StopAsync();
        }

        public static async Task SpeakNow(string Message)
        {
            var settings = new SpeechOptions() { Volume = .75f, Pitch = 1.0f };
            await TextToSpeech.SpeakAsync(Message, settings);
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
