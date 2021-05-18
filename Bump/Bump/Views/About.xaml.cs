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
    public partial class About : CarouselPage
    {
        public ICommand OpenEmailMessageCommand { get; set; }
        public About()
        {
            InitializeComponent();
            OpenEmailMessageCommand = new Command<string>(email=> OpenEmailMessage(email));
            this.BindingContext = this;
        }

        private void OpenEmailMessage(string email)
        {
            var message = new Xamarin.Essentials.EmailMessage
            {
                To = new List<string> { email },
                Subject = "Need to help",
            };
            Xamarin.Essentials.Email.ComposeAsync(message);
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