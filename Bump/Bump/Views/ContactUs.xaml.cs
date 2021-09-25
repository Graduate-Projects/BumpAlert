using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bump.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactUs : ContentPage
    {
        

        public ContactUs()
        {
            InitializeComponent();
            
        }

        [Obsolete]
        protected void FacebookPage(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://www.facebook.com/BUMP-103683792005187/"));
        }

        [Obsolete]
        protected void TwitterPage(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://twitter.com/Bump46783632?s=09"));
        }


        [Obsolete]
        protected void LinkedinPage(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://www.linkedin.com/in/bump-app-647281218"));
        }

    }
}