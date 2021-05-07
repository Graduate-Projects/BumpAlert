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
    public partial class Contribute : FlyoutPage
    {
        public Contribute()
        {
            InitializeComponent();
            this.Flyout = new ContributeFlyout();
            this.Detail = new ContributeDetail();
        }

        
    }
}