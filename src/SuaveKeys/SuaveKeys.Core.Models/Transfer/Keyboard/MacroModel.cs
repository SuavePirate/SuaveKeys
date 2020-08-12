using System;
using System.Collections.Generic;
using System.Text;

namespace SuaveKeys.Core.Models.Transfer.Keyboard
{
    public class MacroModel
    {
        public string Phrase { get; set; }
        public List<MacroEvent> Events { get; set; }
    }
}
