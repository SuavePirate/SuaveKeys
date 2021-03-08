using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceResult;
using SuaveKeys.Clients.Models.Language;
using SuaveKeys.Clients.Models.Luis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SuaveKeys.Clients.Services
{
    public class LuisLanguageUnderstandingService : ILanguageService
    {
        private HttpClient _client;

        public LuisLanguageUnderstandingService(HttpClient client)
        {
            _client = client;
        }

        public async Task<Result<Intent>> ProcessLanguage(string input)
        {
            try
            {
                
                var result = await _client.GetAsync($"https://suavekeys.cognitiveservices.azure.com/luis/prediction/v3.0/apps/fda4acbe-3c37-410d-a630-c66ec1722b12/slots/production/predict?subscription-key={LuisKeys.PredictionKey}&verbose=true&show-all-intents=true&log=true&query={input}");
                if (!result.IsSuccessStatusCode)
                    return new InvalidResult<Intent>("Unable to handle request/response from wit.ai");

                var json = await result.Content.ReadAsStringAsync();
                var luisResponse = JsonConvert.DeserializeObject<LuisPredictionResponse>(json);

                // map to intent
                var model = new Intent
                {
                    Name = luisResponse.Prediction.TopIntent,
                    Slots = luisResponse.Prediction.Entities?.Where(kvp => kvp.Key != "$instance")?.Select(kvp => new Slot
                    {
                        Name = kvp.Key,
                        SlotType = kvp.Key,
                        Value = kvp.Value.FirstOrDefault()?.Value<string>()
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
