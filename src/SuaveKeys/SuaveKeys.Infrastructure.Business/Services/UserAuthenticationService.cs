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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SuaveKeys.Infrastructure.Business.Services
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IAuthClientRepository _authClientRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHashProvider _hashProvider;
        private readonly IAuthorizationCodeRepository _authorizationCodeRepository;
        private readonly IOptions<AuthSettings> _authSettings;

        public UserAuthenticationService(IRefreshTokenRepository refreshTokenRepository, 
            IAuthClientRepository authClientRepository, 
            IUserRepository userRepository, 
            IHashProvider hashProvider, 
            IAuthorizationCodeRepository authorizationCodeRepository, 
            IOptions<AuthSettings> authSettings)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _authClientRepository = authClientRepository;
            _userRepository = userRepository;
            _hashProvider = hashProvider;
            _authorizationCodeRepository = authorizationCodeRepository;
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


        public async Task<bool> RequestAuthentication(AuthCodeRequest request)
        {
            // can we show UI?

            // validate client id
            var authClient = await _authClientRepository.FindById(request.ClientId);
            if (authClient is null)
                throw new Exception("Invalid client.");

            // validate client origin
            if (!request.Origin.ToLower().StartsWith(authClient.OriginHost))
                throw new Exception("Invalid origin.");


            // validate redirect origin
            if (!request.RedirectUri.ToLower().StartsWith(authClient.AuthCodeRedirectUrlHost))
                throw new Exception("Invalid redirect uri.");


            return true;
            
        }

        public async Task<AuthCodeResponse> GrantAuthCode(AuthCodeSignInRequest request)
        {
            // give auth code

            // validate username/password
            var user = await _userRepository.FindByEmail(request.Username);
            if (user is null)
                throw new Exception("Invalid username or password.");

            var isPasswordMatch = _hashProvider.GetPasswordMatch(user.PasswordHash, request.Password);
            if (!isPasswordMatch)
                throw new Exception("Invalid username or password.");

            // re-run request validation
            var isValid = await RequestAuthentication(request);
            if (!isValid)
                throw new Exception("Invalid client.");

            // create auth code and store it
            var authCode = new AuthorizationCode
            {
                AuthClientId = request.ClientId,
                ChallengeCode = request.Challenge,
                ChallengeMethod = request.ChallengeMethod,
                CreatedDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddMinutes(5),
                State = request.State,
                Code = GenerateAuthCode(),
                UserId = user.Id
            };

            await _authorizationCodeRepository.Add(authCode);
            await _authorizationCodeRepository.SaveChangesAsync();

            // return response
            return new AuthCodeResponse
            {
                Code = authCode.Code,
                State = authCode.State
            };
        }

        public async Task<TokenResponse> AuthenticateAuthorizationCode(AuthCodeTokenRequest request)
        {
            // give tokens!

            // find the auth code by code and challenge
            var authCode = await _authorizationCodeRepository.FindByCodeAndClientId(request.Code, request.ClientId);
            if (authCode is null)
                throw new Exception("Invalid code or client");

            if (authCode.ExpirationDate < DateTime.UtcNow)
                throw new Exception("Expired code.");

            // validate challenge code
            if (authCode.ChallengeMethod == "plain" && authCode.ChallengeCode != request.Challenge)
                throw new Exception("Invalid challenge.");
            if(authCode.ChallengeMethod == "s256")
            {
                // hash and base 64 url encode the request.Challenge
                var hashedValue = ComputeSha256Hash(request.Challenge);
                var encoded = Base64UrlEncoder.Encode(hashedValue);

                // compare new hash with stored challenge code and throw if not the same
                if (encoded != authCode.ChallengeCode)
                    throw new Exception("Invalid challenge.");
            }

            // validate the redirect url
            if (!request.RedirectUri.ToLower().StartsWith(authCode.AuthClient.TokenRedirectUrlHost))
                throw new Exception("Invalid redirect uri.");

            // validate grant type
            if (request.GrantType != "authorization_code")
                throw new Exception("Invalid grant type");


            // generate tokens
            // hey you're authenticated! Here are your tokens!
            var accessToken = GenerateToken(authCode.User);
            var refreshToken = new RefreshToken
            {
                UserId = authCode.User?.Id,
                AuthClientId = authCode.AuthClientId,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddDays(_authSettings.Value.RefreshTokenExpirationDays),
                Token = Guid.NewGuid().ToString()
            };
            await _refreshTokenRepository.AddAsync(refreshToken);
            await _authorizationCodeRepository.Remove(authCode);

            await _refreshTokenRepository.SaveChangesAsync();

            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                AccessTokenExpiration = DateTime.UtcNow.AddHours(_authSettings.Value.AccessTokenExpirationHours),
                RefreshTokenExpiration = refreshToken.ExpirationDate
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

        private string GenerateAuthCode(int length = 128)
        {
            // Create a string of characters, numbers, special characters that allowed in the password  
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-._~";
            Random random = new Random();

            // Select one random character at a time from the string  
            // and create an array of chars  
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }


        string ComputeSha256Hash(string originalString)
        {
            // Create a SHA256   
            using (var sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(originalString));

                // Convert byte array to a string   
                var builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

    }
}
