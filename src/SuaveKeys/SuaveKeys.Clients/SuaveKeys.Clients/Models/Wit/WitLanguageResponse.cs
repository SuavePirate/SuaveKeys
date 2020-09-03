using System;
using System.Collections.Generic;
using System.Text;

namespace SuaveKeys.Clients.Models.Wit
{
    public class WitLanguageResponse
    {
        public string Text { get; set; }
        public List<WitIntent> Intents { get; set; }
        public Dictionary<string, WitEntity[]> Entities { get; set; }
    }
}
