using SuaveKeys.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SuaveKeys.Core.Data.Repositories
{
    public interface IAuthorizationCodeRepository
    {
        Task<AuthorizationCode> Add(AuthorizationCode entity);
        Task<AuthorizationCode> Remove(AuthorizationCode entity);
        Task<AuthorizationCode> FindByCodeAndClientId(string code, string clientId);

        Task SaveChangesAsync();
    }
}
