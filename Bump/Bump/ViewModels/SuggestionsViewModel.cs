using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Bump.ViewModels
{
    public class SuggestionsViewModel : BaseViewModel
    {
        public BLL.Models.Suggestion Suggestion { get; set; }
        public ICommand SubmitSuggestionCommand { get; set; }
        public SuggestionsViewModel()
        {
            Suggestion = new BLL.Models.Suggestion();
            SubmitSuggestionCommand = new Command(()=> SubmitSuggestion().ConfigureAwait(false));
        }

        private async Task SubmitSuggestion()
        {
            using (var loading = new Components.LoadingView())
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.PostAsJsonAsync($"{BLL.Settings.Connections.GetServerAddress()}/api/Suggestions", Suggestion);
                if (response.IsSuccessStatusCode)
                {
                    DisplayAlert(string.Empty, "We have received your suggestion and it has been submitted to the competent authorities and working on it\nThank you", "Ok");
                    BackPage();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    DisplayAlert("An occured error", error, "Ok");
                }
            }
        }
    }
}
