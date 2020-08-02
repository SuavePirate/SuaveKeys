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
    public class UserKeyboardProfileRepository : BaseEntityRepository<UserKeyboardProfile>, IUserKeyboardProfileRepository
    {
        public UserKeyboardProfileRepository(SuaveKeysContext context) : base(context)
        {
        }

        public async Task<UserKeyboardProfile> Add(UserKeyboardProfile entity)
        {
            await _data.AddAsync(entity);
            return entity;
        }

        public async Task<UserKeyboardProfile> FindById(string profileId)
        {
            return await _data.Where(u => u.Id == profileId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserKeyboardProfile>> GetForUser(string userId)
        {
            return await _data.Where(u => u.UserId == userId).ToListAsync();
        }

        public Task<UserKeyboardProfile> Remove(UserKeyboardProfile entity)
        {
            _data.Remove(entity);
            return Task.FromResult(entity);
        }
    }
}
