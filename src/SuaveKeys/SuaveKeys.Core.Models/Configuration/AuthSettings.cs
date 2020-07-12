using System;
using System.Collections.Generic;
using System.Text;

namespace SuaveKeys.Core.Models.Configuration
{
    public class AuthSettings
    {
        public string EncodingSecret { get; set; }
        public int AccessTokenExpirationHours { get; set; }
        public int RefreshTokenExpirationDays { get; set; }
        public string Issuer { get; set; }
    }
}
