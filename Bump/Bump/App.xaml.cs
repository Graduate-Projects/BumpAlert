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
        public App()
        {
            InitializeComponent();
            XF.Material.Forms.Material.Init(this);
            localizer.Initialization();
            StartPage().ConfigureAwait(false);
            
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

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
