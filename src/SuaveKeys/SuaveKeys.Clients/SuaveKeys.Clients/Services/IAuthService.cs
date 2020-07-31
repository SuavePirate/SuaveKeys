using ServiceResult;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SuaveKeys.Clients.Services
{
    public interface IAuthService
    {
        Task<Result<bool>> StartAuthentication();
        Task<Result<bool>> RefreshToken();
        Task<Result<string>> GetCurrentAccessToken();
    }
}
