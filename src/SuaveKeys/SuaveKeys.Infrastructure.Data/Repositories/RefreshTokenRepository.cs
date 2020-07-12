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
    public class RefreshTokenRepository : BaseEntityRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(SuaveKeysContext context) : base(context)
        {
        }

        public async Task<RefreshToken> FindByToken(string token)
        {
            return await _data.Include(r => r.User)
                .Where(r => r.Token == token)
                .FirstOrDefaultAsync();
        }

        public async Task<RefreshToken> AddAsync(RefreshToken entity)
        {
            await _data.AddAsync(entity);
            return entity;
        }

        public async Task<RefreshToken> DeleteById(string id)
        {
            var entity = await _data.FirstOrDefaultAsync(r => r.Id == id);
            if(entity != null)
            {
                _data.Remove(entity);
            }

            return entity;
        }
    }
}
