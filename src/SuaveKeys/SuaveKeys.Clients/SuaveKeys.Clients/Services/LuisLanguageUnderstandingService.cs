using ServiceResult;
using SuaveKeys.Clients.Models.Language;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SuaveKeys.Clients.Services
{
    public class LuisLanguageUnderstandingService : ILanguageService
    {
        public Task<Result<Intent>> ProcessLanguage(string input)
        {
            throw new NotImplementedException();
        }
    }
}
