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
    public class UserRepository : BaseEntityRepository<User>, IUserRepository
    {
        public UserRepository(SuaveKeysContext context) : base(context)
        {
        }

        public async Task<User> FindByEmail(string email)
        {
            return await _data.Where(u => u.Email.ToLower() == email.ToLower())
                .FirstOrDefaultAsync();
        }

        public async Task<User> Add(User entity)
        {
            await _data.AddAsync(entity);
            return entity;
        }

    }
}
