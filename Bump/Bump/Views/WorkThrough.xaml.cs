using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using localizer = Bump.Utils.LocalizationResourceManager;

namespace Bump.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkThrough : CarouselPage
    {
        public WorkThrough()
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

        private void ClosePage(object sender, EventArgs e)
        {
            var page = new Views.MainPageView.MainPage();
            page.SetBinding(VisualElement.FlowDirectionProperty, new Binding(nameof(localizer.FlowDirection), source: localizer.Instance));
            Application.Current.MainPage = new NavigationPage(page);
        }
    }
}