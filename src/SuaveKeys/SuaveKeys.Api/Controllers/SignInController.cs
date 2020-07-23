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
            var response = await _authService.RequestAuthentication(model);

            // temp - this is just for manually testing the logic stuff using swagger❤❤❤❤❤❤❤❤
            //return Ok(isValid);
            if (response.ResultType == ServiceResult.ResultType.Ok)
                return View(model);

            return View("ValidationErrors", response.Errors);
        }

        [HttpPost]
        public async Task<IActionResult> Index(AuthCodeSignInRequest request)
        {
            var response = await _authService.GrantAuthCode(request);

            if (response?.ResultType == ServiceResult.ResultType.Ok)
                return View("SignInRedirect", (request.RedirectUri, response.Data.State, response.Data.Code));

            return BadRequest(response.Errors);
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

            if (response?.ResultType == ServiceResult.ResultType.Ok)
                return Redirect($"{redirect_uri}?access_token={response.Data.AccessToken}&access_token_expiration={response.Data.AccessTokenExpiration}&refresh_token={response.Data.RefreshToken}&refresh_token_expiration={response.Data.RefreshTokenExpiration}");

            return View("ValidationErrors", response.Errors);
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

            if (response.ResultType == ServiceResult.ResultType.Ok)
                return Ok(new
                {
                    access_token = response.Data.AccessToken,
                    access_token_expiration = response.Data.AccessTokenExpiration,
                    refresh_token = response.Data.RefreshToken
                });

            return BadRequest(response.Errors);
        }
    }
}
