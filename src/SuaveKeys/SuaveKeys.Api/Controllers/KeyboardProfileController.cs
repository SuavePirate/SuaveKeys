using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuaveKeys.Core.Business.Services;
using SuaveKeys.Core.Models.Transfer.Keyboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SuaveKeys.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class KeyboardProfileController : Controller
    {
        private readonly IKeyboardProfileService _keyboardProfileService;

        public KeyboardProfileController(IKeyboardProfileService keyboardProfileService)
        {
            _keyboardProfileService = keyboardProfileService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name, [FromBody]KeyboardProfileConfiguration model)
        {
            var result = await _keyboardProfileService.CreateNewConfiguration(GetUserId(), name, model);
            if (result?.ResultType == ServiceResult.ResultType.Ok)
                return Ok(result.Data);

            return BadRequest(result.Errors);
        }

        [HttpDelete("{profileId}")]
        public async Task<IActionResult> Delete(string profileId)
        {
            var result = await _keyboardProfileService.DeleteConfiguration(GetUserId(),profileId);
            if (result?.ResultType == ServiceResult.ResultType.Ok)
                return Ok(result.Data);

            return BadRequest(result.Errors);
        }

        [HttpGet]
        public async Task<IActionResult> GetForUser()
        {
            var result = await _keyboardProfileService.GetConfigurationsForUser(GetUserId());
            if (result?.ResultType == ServiceResult.ResultType.Ok)
                return Ok(result.Data);

            return BadRequest(result.Errors);
        }


        private string GetUserId()
        {
            return User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
        }
    }
}
