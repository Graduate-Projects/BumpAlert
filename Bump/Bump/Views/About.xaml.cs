using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bump.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class About : CarouselPage
    {
        public About()
        {
            InitializeComponent();
        }
        private void SkipMove(object sender, EventArgs e)
        {
            this.CurrentPage = this.Children.Last();
        }

        private void NextMove(object sender, EventArgs e)
        {
            var indexCurrentPageSelected = GetIndex(this.CurrentPage);
            this.CurrentPage = GetPageByIndex(indexCurrentPageSelected + 1);
        }

        private void CloseAboutPage(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PopToRootAsync();
        }
    }
}