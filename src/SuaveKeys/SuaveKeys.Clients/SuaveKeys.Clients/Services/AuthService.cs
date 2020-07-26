using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Newtonsoft.Json;
using ServiceResult;
using SuaveKeys.Core.Models.Transfer.Authentication;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SuaveKeys.Clients.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthClientSettings _clientSettings;
        private string _currentToken;
        private string _baseUrl = "https://suavekeys-dev.azurewebsites.net";
        private const string TokenInfoKey = "TOKEN_INFO";
        public AuthService()
        {
            _clientSettings = DependencyService.Get<IAuthClientSettings>();
        }

        public async Task<Result<string>> GetCurrentAccessToken()
        {
            try
            {
                if (!string.IsNullOrEmpty(_currentToken))
                    return new SuccessResult<string>(_currentToken);

                var refreshResult = await RefreshToken();
                if (refreshResult?.ResultType == ResultType.Ok)
                    return new SuccessResult<string>(_currentToken);

                return new InvalidResult<string>(refreshResult.Errors?.FirstOrDefault());

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return new UnexpectedResult<string>();
            }
        }

        public async Task<Result<bool>> RefreshToken()
        {
            try
            {
                var tokenJson = await SecureStorage.GetAsync(TokenInfoKey);
                if (string.IsNullOrEmpty(tokenJson))
                    return new InvalidResult<bool>("No token info. You are not signed in.");

                var tokenInfo = JsonConvert.DeserializeObject<TokenResponse>(tokenJson);


                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync($"{_baseUrl}/signin/token?client_id={_clientSettings.ClientId}&client_secret={_clientSettings.ClientSecret}&refresh_token={tokenInfo.RefreshToken}&grant_type=refresh_token&redirect_uri=suavekeys://", null);
                    if (response?.IsSuccessStatusCode == true)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        tokenInfo = JsonConvert.DeserializeObject<TokenResponse>(json);

                        // we have our tokens. Gotta do something with it
                        _currentToken = tokenInfo?.AccessToken;
                        await StoreTokenInfo(tokenInfo);

                        return new SuccessResult<bool>(true);
                    }
                }
                return new InvalidResult<bool>("Unable to authenticate.");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new UnexpectedResult<bool>();
            }
        }

        public async Task<Result<bool>> StartAuthentication()
        {
            try
            {
                var state = Guid.NewGuid().ToString();
                var clientId = _clientSettings.ClientId;
                var authResult = await WebAuthenticator.AuthenticateAsync(
                    new Uri($"{_baseUrl}/signin?client_id={clientId}&state={state}&redirect_uri=suavekeys://"),
                    new Uri("suavekeys://"));

                var code = authResult.Properties["code"];
                var confirmState = authResult.Properties["state"];

                if (state != confirmState)
                    await UserDialogs.Instance.AlertAsync("Invalid state. This might be a sign of an interception attack.");

                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync($"{_baseUrl}/signin/token?client_id={clientId}&code={code}&grant_type=authorization_code&redirect_uri=suavekeys://", null);
                    if (response?.IsSuccessStatusCode == true)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var tokenInfo = JsonConvert.DeserializeObject<TokenResponse>(json);

                        // we have our tokens. Gotta do something with it
                        _currentToken = tokenInfo?.AccessToken;
                        await StoreTokenInfo(tokenInfo);

                        return new SuccessResult<bool>(true);
                    }
                }
                

                return new InvalidResult<bool>("Unable to authenticate.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new UnexpectedResult<bool>();
            }
        }

        private async Task StoreTokenInfo(TokenResponse tokenInfo)
        {
            await SecureStorage.SetAsync(TokenInfoKey, JsonConvert.SerializeObject(tokenInfo));
        }
    }
}
