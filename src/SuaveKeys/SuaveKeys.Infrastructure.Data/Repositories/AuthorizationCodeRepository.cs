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
    public class AuthorizationCodeRepository : BaseEntityRepository<AuthorizationCode>, IAuthorizationCodeRepository
    {
        public AuthorizationCodeRepository(SuaveKeysContext context) : base(context)
        {
        }

        public async Task<AuthorizationCode> Add(AuthorizationCode entity)
        {
            await _data.AddAsync(entity);
            return entity;
        }

        public async Task<AuthorizationCode> FindByCodeAndClientId(string code, string clientId)
        {
            return await _data.Include(a => a.User)
                .Include(a => a.AuthClient)
                .Where(a => a.Code == code && a.AuthClientId == clientId)
                .FirstOrDefaultAsync();
        }

        public Task<AuthorizationCode> Remove(AuthorizationCode entity)
        {
            _data.Remove(entity);
            return Task.FromResult(entity);
        }
    }
}
