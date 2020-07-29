using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SuaveKeys.Api.Hubs;
using SuaveKeys.Core.Models.Transfer.Voicify.Input;
using SuaveKeys.Core.Models.Transfer.Voicify.Output;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SuaveKeys.Api.Controllers
{
    [Route("api/[controller]")]
    public class VoicifyController : ControllerBase
    {
        private readonly IHubContext<KeyboardHub> _keyboardHub;

        public VoicifyController(IHubContext<KeyboardHub> keyboardHub)
        {
            _keyboardHub = keyboardHub;
        }

        [HttpPost("PressKeyIntent")]
        public async Task<VoicifyResponse> PressKey([FromBody]VoicifyRequest request)
        {
            var token = new JwtSecurityTokenHandler().ReadJwtToken(request.OriginalRequest.AccessToken);
            if (!IsTokenValid(token))
                return new VoicifyResponse
                {
                    Data = new VoicifyResponseData
                    {
                        Content = "You are not signed in. Please authenticate to press a key"
                    }
                };
            //var email = token?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var userClients = _keyboardHub.Clients.User(token?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "error");

            await userClients.SendAsync("KEY_PRESS", request.OriginalRequest.Slots["key"]);

            return new VoicifyResponse
            {
                Data = new VoicifyResponseData
                {
                    // We don't need to send something down if it worked. Use the response in Voicify
                }
            };
        }

        [HttpPost("TypeIntent")]
        public async Task<VoicifyResponse> Type([FromBody]VoicifyRequest request)
        {
            var token = new JwtSecurityTokenHandler().ReadJwtToken(request.OriginalRequest.AccessToken);
            if (!IsTokenValid(token))
                return new VoicifyResponse
                {
                    Data = new VoicifyResponseData
                    {
                        Content = "You are not signed in. Please authenticate to type"
                    }
                };
            var userClients = _keyboardHub.Clients.User(token?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "error");

            await userClients.SendAsync("TYPE", request.OriginalRequest.Slots["phrase"]);

            return new VoicifyResponse
            {
                Data = new VoicifyResponseData
                {
                    Content = $"Typing {request.OriginalRequest.Slots["phrase"]}"
                }
            };
        }

        private bool IsTokenValid(JwtSecurityToken token)
        {
            var expiration = token?.Claims?.FirstOrDefault(c => c.Type == "expiration_date")?.Value;
            if (expiration == null)
                return false;

            var isValidDate = DateTime.TryParse(expiration, out var expirationDate);
            if (!isValidDate)
                return false;

            return expirationDate < DateTime.UtcNow;
        }
    }
}
