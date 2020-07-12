using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuaveKeys.Core.Business.Services;
using SuaveKeys.Core.Models.Transfer.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SuaveKeys.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserAuthenticationService _authService;

        public AuthenticationController(IUserAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]AuthenticationRequest model)
        {
            try
            {
                var response = await _authService.AuthenticateUser(model);
                return Ok(response);
            }
            catch(Exception ex)
            {
                // TODO: refactor to result pattern
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("test")]
        [Authorize]
        public string AuthTest()
        {
            return User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
