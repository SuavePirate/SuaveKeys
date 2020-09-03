using System;
using System.Collections.Generic;
using System.Text;

namespace SuaveKeys.Clients.Models.Language
{
    public class Intent
    {
        public string Name { get; set; }
        public Slot[] Slots { get; set; }
    }
}
