using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using ServiceResult;
using SuaveKeys.Clients.Models;
using SuaveKeys.Clients.Models.Language;
using SuaveKeys.Clients.Models.Wit;

namespace SuaveKeys.Clients.Services
{
    public class WitLanguageUnderstandingService : ILanguageService
    {
        private readonly HttpClient _client;

        public WitLanguageUnderstandingService(HttpClient client)
        {
            _client = client;
        }

        public async Task<Result<Intent>> ProcessLanguage(string input)
        {
            try
            {
                if (_client.DefaultRequestHeaders.Contains("Authorization"))
                    _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {WitKeys.WitAccessKey}");
                var result = await _client.GetAsync($"https://api.wit.ai/message?v=1&q={HttpUtility.UrlEncode(input)}");
                if (!result.IsSuccessStatusCode)
                    return new InvalidResult<Intent>("Unable to handle request/response from wit.ai");

                var json = await result.Content.ReadAsStringAsync();
                var witResponse = JsonConvert.DeserializeObject<WitLanguageResponse>(json);

                // map to intent
                var model = new Intent
                {
                    Name = witResponse.Intents.FirstOrDefault()?.Name,
                    Slots = witResponse.Entities.Select(kvp => new Slot
                    {
                        Name = kvp.Value.FirstOrDefault()?.Name,
                        SlotType = kvp.Value.FirstOrDefault()?.Type,
                        Value = kvp.Value.FirstOrDefault()?.Value
                    }).ToArray()
                };

                return new SuccessResult<Intent>(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new UnexpectedResult<Intent>();
            }
        }
    }
}
