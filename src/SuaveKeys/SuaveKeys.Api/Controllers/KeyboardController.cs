using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SuaveKeys.Api.Hubs;
using SuaveKeys.Core.Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SuaveKeys.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class KeyboardController : ControllerBase
    {
        private readonly IHubContext<KeyboardHub> _keyboardHub;

        public KeyboardController(IHubContext<KeyboardHub> keyboardHub)
        {
            _keyboardHub = keyboardHub;
        }

        [HttpPost("KeyPress")]
        public async Task KeyPress(string key)
        {
            var userClients = _keyboardHub.Clients.User(User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "error");
            await userClients.SendAsync(KeyboardEvents.PressKey, key);
        }

        [HttpPost("Type")]
        public async Task TypePhrase(string phrase)
        {
            var userClients = _keyboardHub.Clients.User(User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "error");
            await userClients.SendAsync(KeyboardEvents.Type, phrase);
        }
    }
}
