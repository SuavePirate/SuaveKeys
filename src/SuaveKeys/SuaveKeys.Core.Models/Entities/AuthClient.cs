﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SuaveKeys.Core.Models.Entities
{
    public class AuthClient
    {
        [Required]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Secret { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public DateTime ModifiedDate { get; set; }
        public string AuthCodeRedirectUrlHost { get; set; } // used for auth code grant flow
        public string TokenRedirectUrlHost { get; set; } // used for validation
        public string OriginHost { get; set; }

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
