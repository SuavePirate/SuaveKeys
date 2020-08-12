using System;
using System.Collections.Generic;
using System.Text;

namespace SuaveKeys.Core.Models.Transfer.Keyboard
{
    /// <summary>
    /// Contains information about a single keyboard event. Should only have one of each of the given properties
    /// </summary>
    public class MacroEvent
    {
        public MacroEventType EventType { get; set; }
        public string Key { get; set; }
        public int HoldTimeMilliseconds { get; set; }
        public string TypedPhrase { get; set; }
    }
}
