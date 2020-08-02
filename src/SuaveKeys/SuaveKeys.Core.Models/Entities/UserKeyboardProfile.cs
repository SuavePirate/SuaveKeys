using System;
using System.Collections.Generic;
using System.Text;

namespace SuaveKeys.Core.Models.Entities
{
    public class UserKeyboardProfile
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public string ConfigurationJson { get; set; }

        // navigation 
        public User User { get; set; }
    }
}
