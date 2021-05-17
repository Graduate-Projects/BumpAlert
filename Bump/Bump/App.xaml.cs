using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
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

            var page = new Views.MainPageView.MainPage();
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
