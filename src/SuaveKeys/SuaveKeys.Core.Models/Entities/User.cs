using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuaveKeys.Core.Models.Entities
{
    public class User 
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
