using System;
using System.Collections.Generic;
using System.Text;

namespace SuaveKeys.Core.Models.Transfer.Authentication
{
    /// <summary>
    /// Response model for returning an auth code after a valid request
    /// </summary>
    public class AuthCodeResponse
    {
        public string Code { get; set; }
        public string State { get; set; }
    }
}
