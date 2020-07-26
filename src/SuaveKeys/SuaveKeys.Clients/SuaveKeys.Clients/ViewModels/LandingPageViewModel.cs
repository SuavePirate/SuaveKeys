using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SuaveKeys.Clients.ViewModels
{
    public class LandingPageViewModel : BaseViewModel
    {
        public ICommand SignInCommand { get; set; }
        public LandingPageViewModel()
        {
            Title = "Welcome to Suave Keys";
            SignInCommand = new Command(async () =>
            {
                var clientId = "2b1e452c-322e-4c26-ba07-4a9a08574766";
                var state = Guid.NewGuid().ToString();

                try
                {
                    var authResult = await WebAuthenticator.AuthenticateAsync(
                        new Uri($"https://suavekeys-dev.azurewebsites.net/signin?client_id={clientId}&state={state}&redirect_uri=suavekeys://"),
                        new Uri("suavekeys://"));

                    var code = authResult.Properties["code"];
                    var confirmState = authResult.Properties["state"];

                    if (state != confirmState)
                        await UserDialogs.Instance.AlertAsync("Invalid state. This might be a sign of an interception attack.");

                    var client = new HttpClient();
                    var response = await client.PostAsync($"https://suavekeys-dev.azurewebsites.net/signin/token?client_id={clientId}&code={code}&grant_type=authorization_code&redirect_uri=suavekeys://", null);
                    if(response?.IsSuccessStatusCode == true)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(json);
                    }

                    Console.WriteLine(authResult);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            });
        }
    }
}
