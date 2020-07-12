using SuaveKeys.Core.Models.Transfer.Authentication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SuaveKeys.Core.Business.Services
{
    public interface IUserAuthenticationService
    {
        /// <summary>
        /// Authenticate the user with either password or refresh token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<TokenResponse> AuthenticateUser(AuthenticationRequest request);
    }
}
