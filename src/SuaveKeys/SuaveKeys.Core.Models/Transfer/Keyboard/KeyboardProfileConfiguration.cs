using System;
using System.Collections.Generic;
using System.Text;

namespace SuaveKeys.Core.Models.Transfer.Keyboard
{
    public class KeyboardProfileConfiguration
    {
        /// <summary>
        /// Contains a mapping of commands to keyboard keys. 
        /// The key of the dictionary is the command, and the value is the keyboard key
        /// </summary>
        public Dictionary<string, string> CommandKeyMappings { get; set; }

        // NOTE: when we implement macros, those will go here too

    }
}
