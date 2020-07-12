using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SuaveKeys.Core.Business.Services;
using SuaveKeys.Core.Data.Providers;
using SuaveKeys.Core.Data.Repositories;
using SuaveKeys.Core.Models.Configuration;
using SuaveKeys.Core.Models.Entities;
using SuaveKeys.Core.Models.Transfer.Authentication;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SuaveKeys.Infrastructure.Business.Services
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IAuthClientRepository _authClientRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHashProvider _hashProvider;
        private readonly IOptions<AuthSettings> _authSettings;

        public UserAuthenticationService(IRefreshTokenRepository refreshTokenRepository, IAuthClientRepository authClientRepository, IUserRepository userRepository, IHashProvider hashProvider, IOptions<AuthSettings> authSettings)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _authClientRepository = authClientRepository;
            _userRepository = userRepository;
            _hashProvider = hashProvider;
            _authSettings = authSettings;
        }

        public async Task<TokenResponse> AuthenticateUser(AuthenticationRequest request)
        {
            var authClient = await _authClientRepository.FindByIdAndSecret(request.ClientId, request.ClientSecret);
            if (authClient == null)
                throw new Exception("Invalid client");

            var user = await _userRepository.FindByEmail(request.Username);
            if (user is null)
                throw new Exception("Invalid username or password.");

            if(request.GrantType == "password")
            {
                var isPasswordMatch = _hashProvider.GetPasswordMatch(user.PasswordHash, request.Password);
                if (!isPasswordMatch)
                    throw new Exception("Invalid username or password.");
            }
            else if (request.GrantType == "refresh_token")
            {
                var existingToken = await _refreshTokenRepository.FindByToken(request.RefreshToken);
                if (existingToken is null || existingToken.AuthClient.Id != request.ClientId || existingToken.AuthClient.Secret != request.ClientSecret)
                    throw new Exception("Invalid token or client");

                await _refreshTokenRepository.DeleteById(existingToken.Id);
            }
            else
            {
                throw new Exception("Invalid grant type");
            }

            // hey you're authenticated! Here are your tokens!
            var accessToken = GenerateToken(user);
            var refreshToken = new RefreshToken
            {
                UserId = user?.Id,
                AuthClientId = authClient.Id,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddDays(_authSettings.Value.RefreshTokenExpirationDays),
                Token = Guid.NewGuid().ToString()
            };
            await _refreshTokenRepository.AddAsync(refreshToken);
            await _refreshTokenRepository.SaveChangesAsync();

            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                AccessTokenExpiration = DateTime.UtcNow.AddHours(_authSettings.Value.AccessTokenExpirationHours),
                RefreshTokenExpiration = refreshToken.ExpirationDate,
                UserId = user.Id
            };

        }

        private string GenerateToken(User user)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Expiration, _authSettings.Value.AccessTokenExpirationHours.ToString()),
            };

            var symmetricKeyAsBase64 = _authSettings.Value.EncodingSecret;
            var keyByteArray = Encoding.UTF8.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            var jwt = new JwtSecurityToken(
                issuer: _authSettings.Value.Issuer,
                audience: _authSettings.Value.Issuer,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(_authSettings.Value.AccessTokenExpirationHours),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));


            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }
    }
}
