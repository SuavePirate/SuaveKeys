using Microsoft.AspNetCore.Mvc;
using SuaveKeys.Core.Business.Services;
using SuaveKeys.Core.Models.Transfer.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuaveKeys.Api.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody]NewUserRequest model)
        {
            var result = await _userService.CreateUser(model);
            return Ok(result);
        }
    }
}
