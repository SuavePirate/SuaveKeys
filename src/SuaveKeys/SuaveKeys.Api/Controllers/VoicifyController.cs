using Microsoft.AspNetCore.Mvc;
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
            return new VoicifyResponse
            {
                Data = new VoicifyResponseData
                {
                    Content = $"Typing {request.OriginalRequest.Slots["phrase"]}"
                }
            };
        }
    }
}
