using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms;
using XF.Material.Forms.UI;

namespace Bump.Views.MainPageView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : FlyoutPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.Flyout = new MainPageFlyout();
            this.Detail = new MaterialNavigationPage(new MainPageDetail());
        }
    }
}