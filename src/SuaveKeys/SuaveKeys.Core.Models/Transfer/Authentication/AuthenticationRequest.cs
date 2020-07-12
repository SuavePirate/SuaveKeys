using System;
using System.Collections.Generic;
using System.Text;

namespace SuaveKeys.Core.Models.Transfer.Authentication
{
    public class AuthenticationRequest
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Username { get; set; }
        public string GrantType { get; set; } // password, refresh_token


        public string Password { get; set; }
        public string RefreshToken { get; set; }
    }
}
