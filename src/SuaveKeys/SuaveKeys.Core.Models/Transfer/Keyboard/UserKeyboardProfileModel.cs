using Newtonsoft.Json;
using SuaveKeys.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuaveKeys.Core.Models.Transfer.Keyboard
{
    public class UserKeyboardProfileModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public KeyboardProfileConfiguration Configuration { get; set; }

        public UserKeyboardProfileModel()
        {
        }

        public UserKeyboardProfileModel(UserKeyboardProfile entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            UserId = entity.UserId;
            Configuration = JsonConvert.DeserializeObject<KeyboardProfileConfiguration>(entity.ConfigurationJson);
        }

    }
}
