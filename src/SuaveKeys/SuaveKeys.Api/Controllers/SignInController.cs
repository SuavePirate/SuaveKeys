using Microsoft.AspNetCore.Mvc;
using SuaveKeys.Core.Business.Services;
using SuaveKeys.Core.Models.Transfer.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuaveKeys.Api.Controllers
{
    [Route("[controller]")]
    public class SignInController : Controller
    {
        private readonly IUserAuthenticationService _authService;

        public SignInController(IUserAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string grant_type, string client_id, string redirect_uri, string code_challenge, string state, string code_challenge_method = "plain")
        {
            var model = new AuthCodeRequest
            {
                Challenge = code_challenge,
                State = state,
                ClientId = client_id,
                GrantType = grant_type,
                RedirectUri = redirect_uri,
                ChallengeMethod = code_challenge_method,
                Origin = Request.Headers["Host"]
            };
            var isValid = await _authService.RequestAuthentication(model);

            // TODO: actually check `isValid`

            // temp - this is just for manually testing the logic stuff using swagger
            //return Ok(isValid);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(AuthCodeSignInRequest request)
        {
            var response = await _authService.GrantAuthCode(request);
            // temp
            //return Ok($"{request.RedirectUri}?state={response.State}&code={response.Code}");

            return View("SignInRedirect", (request.RedirectUri, response.State, response.Code));
        }

        [HttpGet("temp-redirect")]
        public async Task<IActionResult> Temp(string state, string code)
        {
            return View("Temp", code);
        }


        [HttpGet("code")]
        public async Task<IActionResult> GrantCode(string grant_type, string client_id, string redirect_uri, string code_verifier, string code)
        {
            var response = await _authService.AuthenticateAuthorizationCode(new AuthCodeTokenRequest
            {
               Challenge = code_verifier,
               ClientId = client_id,
               Code = code,
               GrantType = grant_type,
               RedirectUri = redirect_uri
            });

            // temp
            //return Ok($"{redirect_uri}?access_token={response.AccessToken}&access_token_expiration={response.AccessTokenExpiration}&refresh_token={response.RefreshToken}&refresh_token_expiration={response.RefreshTokenExpiration}");

            return Redirect($"{redirect_uri}?access_token={response.AccessToken}&access_token_expiration={response.AccessTokenExpiration}&refresh_token={response.RefreshToken}&refresh_token_expiration={response.RefreshTokenExpiration}");
        }

        [HttpPost("token")]
        public async Task<IActionResult> GetToken(string grant_type, string client_id, string client_secret, string redirect_uri, string code_verifier, string code, string refresh_token)
        {
            var response = await _authService.AuthenticateAuthorizationCode(new AuthCodeTokenRequest
            {
                Challenge = code_verifier,
                ClientId = client_id,
                Code = code,
                GrantType = grant_type,
                RedirectUri = redirect_uri
            });

            // temp
            //return Ok($"{redirect_uri}?access_token={response.AccessToken}&access_token_expiration={response.AccessTokenExpiration}&refresh_token={response.RefreshToken}&refresh_token_expiration={response.RefreshTokenExpiration}");
            return Ok(new
            {
                access_token = response.AccessToken,
                access_token_expiration = response.AccessTokenExpiration,
                refresh_token = response.RefreshToken
            });
            //return Redirect($"{redirect_uri}?access_token={response.AccessToken}&access_token_expiration={response.AccessTokenExpiration}&refresh_token={response.RefreshToken}&refresh_token_expiration={response.RefreshTokenExpiration}");
        }
    }
}
