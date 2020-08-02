using SuaveKeys.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SuaveKeys.Core.Data.Repositories
{
    public interface IUserKeyboardProfileRepository
    {
        Task<UserKeyboardProfile> FindById(string profileId);
        Task<IEnumerable<UserKeyboardProfile>> GetForUser(string userId);
        Task<UserKeyboardProfile> Add(UserKeyboardProfile entity);
        Task<UserKeyboardProfile> Remove(UserKeyboardProfile entity);
        Task SaveChangesAsync();
    }
}
