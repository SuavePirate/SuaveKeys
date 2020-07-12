using SuaveKeys.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SuaveKeys.Core.Data.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> FindByToken(string token);
        Task<RefreshToken> AddAsync(RefreshToken entity);
        Task<RefreshToken> DeleteById(string id);
        Task SaveChangesAsync();
    }
}
