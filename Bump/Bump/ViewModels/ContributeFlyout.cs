using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Bump.ViewModels
{
    class ContributeFlyout : BaseViewModel
    {
        public ICommand OpenMainPageCommand { get; set; }

        public ContributeFlyout()
        {
            OpenMainPageCommand = new Command(OpenMainPage);
        }

        private void OpenMainPage()
        {
            OpenPage(new Views.MainPageView.MainPage());
        }
    }
}
