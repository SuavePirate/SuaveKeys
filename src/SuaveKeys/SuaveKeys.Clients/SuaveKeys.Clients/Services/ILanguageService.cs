using ServiceResult;
using SuaveKeys.Clients.Models.Language;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SuaveKeys.Clients.Services
{
    public interface ILanguageService
    {
        Task<Result<Intent>> ProcessLanguage(string input);
    }
}
