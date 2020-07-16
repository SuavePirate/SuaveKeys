using System;
using System.Collections.Generic;
using System.Text;

namespace SuaveKeys.Core.Models.Entities
{
    public class AuthorizationCode
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Code { get; set; } // actual value we pass up and down
        public string ChallengeCode { get; set; } // used for validation
        public string AuthClientId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpirationDate { get; set; } // used for validation
        public string State { get; set; }
        public string ChallengeMethod { get; set; } // plain or SHA256
        public string UserId { get; set; }

        // navigation property
        public AuthClient AuthClient { get; set; }
        public User User { get; set; }
    }
}
