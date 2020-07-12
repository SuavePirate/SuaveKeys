using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SuaveKeys.Core.Models.Entities
{
    public class RefreshToken
    {
        [Required]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Token { get; set; }

        public DateTime ExpirationDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string AuthClientId { get; set; }


        // navigation properties
        public User User { get; set; }
        public AuthClient AuthClient { get; set; }

    }
}
