using System;
using System.Collections.Generic;
using System.Text;

namespace SuaveKeys.Core.Models.Transfer.Authentication
{
    /// <summary>
    /// Input model for requesting an auth code
    /// </summary>
    public class AuthCodeRequest
    {
        public string ClientId { get; set; }
        public string RedirectUri { get; set; }
        public string Challenge { get; set; }
        public string GrantType { get; set; }
        public string ChallengeMethod { get; set; }
        public string Origin { get; set; }
        public string State { get; set; }
    }
}
