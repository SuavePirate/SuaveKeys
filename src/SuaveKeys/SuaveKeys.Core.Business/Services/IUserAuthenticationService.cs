﻿using SuaveKeys.Core.Models.Transfer.Authentication;
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

        /// <summary>
        /// Request ability to authenticate user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> RequestAuthentication(AuthCodeRequest request);

        /// <summary>
        /// Validates an auth code request and returns an auth code response
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<AuthCodeResponse> GrantAuthCode(AuthCodeSignInRequest request);

        /// <summary>
        /// Authenticates and authorization code request, validates the code challenge, and returns the tokens
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<TokenResponse> AuthenticateAuthorizationCode(AuthCodeTokenRequest request);
    }
}
