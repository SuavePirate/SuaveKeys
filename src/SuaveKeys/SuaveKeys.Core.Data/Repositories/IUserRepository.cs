using SuaveKeys.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SuaveKeys.Core.Data.Repositories
{
    public interface IUserRepository
    {
        Task<User> FindByEmail(string email);
        Task<User> Add(User entity);
        Task SaveChangesAsync();
    }
}
