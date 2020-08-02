using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SuaveKeys.Core.Models.Transfer.Keyboard;

namespace SuaveKeys.Clients.Services
{
    public class KeyboardProfileService : IKeyboardProfileService
    {
        private readonly IAuthService _authService;
        private const string BASE_PATH = "https://suavekeys-dev.azurewebsites.net/";
        public ObservableCollection<UserKeyboardProfileModel> Profiles { get; private set; }

        public UserKeyboardProfileModel CurrentRunningProfile { get; set; }

        public KeyboardProfileService(IAuthService authService)
        {
            _authService = authService;
            Profiles = new ObservableCollection<UserKeyboardProfileModel>();
        }

        public async Task AddNewProfile(string name, KeyboardProfileConfiguration model)
        {
            using (var client = new HttpClient())
            {
                var accessToken = await _authService.GetCurrentAccessToken();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken.Data}");
                var response = await client.PostAsync($"{BASE_PATH}/api/keyboardprofile?name={name}",
                    new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var config = JsonConvert.DeserializeObject<UserKeyboardProfileModel>(responseJson);
                    Profiles.Add(config);
                }
            }
        }

        public async Task LoadProfilesAsync()
        {
            using (var client = new HttpClient())
            {
                var accessToken = await _authService.GetCurrentAccessToken();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken.Data}");
                var response = await client.GetAsync($"{BASE_PATH}/api/keyboardprofile");

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var configs = JsonConvert.DeserializeObject<List<UserKeyboardProfileModel>>(responseJson);
                    Profiles.Clear();
                    foreach (var config in configs)
                        Profiles.Add(config);
                }
            }
        }
    }
}
