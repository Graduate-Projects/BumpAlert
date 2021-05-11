using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace Bump.ViewModels
{
    public class AboutViewModel : CarouselPage
    {
        public ICommand SkipMoveCommand { get; set; }
        public ICommand NextMoveCommand { get; set; }
        public ICommand CloseAboutPageCommand { get; set; }

        public AboutViewModel()
        {
            SkipMoveCommand = new Command(SkipMove);
            NextMoveCommand = new Command(NextMove);
            CloseAboutPageCommand = new Command(CloseAboutPage);

        }
        private void SkipMove()
        {
            this.CurrentPage = this.Children.Last();
        }

        private void NextMove()
        {
            var indexCurrentPageSelected = GetIndex(this.CurrentPage);
            this.CurrentPage = GetPageByIndex(indexCurrentPageSelected + 1);
        }

        private void CloseAboutPage()
        {
            App.Current.MainPage.Navigation.PopToRootAsync();
        }
    }


}

