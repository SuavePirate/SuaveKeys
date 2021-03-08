using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuaveKeys.Clients.Models.Luis
{
    public class LuisPrediction
    {
        public string TopIntent { get; set; }
        public Dictionary<string, object> Intents { get; set; }
        public Dictionary<string, JArray> Entities { get; set; }
    }
}
