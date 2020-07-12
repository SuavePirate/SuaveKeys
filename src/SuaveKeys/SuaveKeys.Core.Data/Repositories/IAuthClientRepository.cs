using SuaveKeys.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SuaveKeys.Core.Data.Repositories
{
    public interface IAuthClientRepository
    {
        Task<AuthClient> FindByIdAndSecret(string id, string secret);
    }
}
