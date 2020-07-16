using System;
using System.Collections.Generic;
using System.Text;

namespace SuaveKeys.Core.Models.Transfer.Authentication
{
    /// <summary>
    /// Input model for requesting an auth code
    /// </summary>
    public class AuthCodeSignInRequest : AuthCodeRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
