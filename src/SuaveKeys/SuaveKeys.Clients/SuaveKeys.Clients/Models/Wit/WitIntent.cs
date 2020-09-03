using System;
using System.Collections.Generic;
using System.Text;

namespace SuaveKeys.Clients.Models.Wit
{
    public class WitIntent
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Confidence { get; set; }
    }
}
