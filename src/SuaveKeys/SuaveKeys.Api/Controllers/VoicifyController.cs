﻿using Microsoft.AspNetCore.Mvc;
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
        [HttpPost("PressKeyIntent")]
        public VoicifyResponse PressKey([FromBody]VoicifyRequest request)
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
            var email = token?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            return new VoicifyResponse
            {
                Data = new VoicifyResponseData
                {
                    Content = $"Pressing {request.OriginalRequest.Slots["key"]} as {email ?? "unkown"}"
                }
            };
        }

        [HttpPost("TypeIntent")]
        public VoicifyResponse Type([FromBody]VoicifyRequest request)
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
