using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bump.Views.MainPageContribute
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContributeDetail : ContentPage
    {
        public ContributeDetail()
        {
            InitializeComponent();
            PrepareGoogleMap().ConfigureAwait(false);
        }

        private async Task PrepareGoogleMap()
        {
            var LocationWhenInUsePermission = await Xamarin.Essentials.Permissions.RequestAsync<Xamarin.Essentials.Permissions.LocationWhenInUse>();
            if (LocationWhenInUsePermission == Xamarin.Essentials.PermissionStatus.Granted)
            {
                var GoogleMap = new Xamarin.Forms.GoogleMaps.Map()
                {
                    MyLocationEnabled = true,
                };
                GoogleMap.UiSettings.MyLocationButtonEnabled = true;
                this.Content = GoogleMap;
            }
        }

    }
}