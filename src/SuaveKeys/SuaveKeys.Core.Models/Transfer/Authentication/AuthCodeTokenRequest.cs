using System;
using System.Collections.Generic;
using System.Text;

namespace SuaveKeys.Core.Models.Transfer.Authentication
{
    /// <summary>
    /// Request with an auth code to get an access token
    /// </summary>
    public class AuthCodeTokenRequest
    {
        public string Code { get; set; }
        public string Challenge { get; set; }
        public string RedirectUri { get; set; }
        public string GrantType { get; set; }
        public string ClientId { get; set; }
    }
}
