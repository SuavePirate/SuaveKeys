using Microsoft.EntityFrameworkCore;
using SuaveKeys.Core.Data.Repositories;
using SuaveKeys.Core.Models.Entities;
using SuaveKeys.Infrastructure.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuaveKeys.Infrastructure.Data.Repositories
{
    public class AuthClientRepository : BaseEntityRepository<AuthClient>, IAuthClientRepository
    {
        public AuthClientRepository(SuaveKeysContext context) : base(context)
        {
        }

        public async Task<AuthClient> FindByIdAndSecret(string id, string secret)
        {
            return await _data.Where(a => a.Id == id && a.Secret == secret)
                .FirstOrDefaultAsync();
        }
    }
}
