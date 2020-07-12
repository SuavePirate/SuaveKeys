using System;
using System.Collections.Generic;
using System.Text;

namespace SuaveKeys.Core.Models.Transfer.Authentication
{
    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime AccessTokenExpiration { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
        public string UserId { get; set; }
    }
}
