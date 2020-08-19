using SuaveKeys.Core.Models.Transfer.Keyboard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SuaveKeys.Clients.ViewModels
{
    public class MacroEventViewModel : MacroEvent, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsKey => EventType == MacroEventType.KeyPress;
        public bool IsType => EventType == MacroEventType.Type;
        public bool IsPause => EventType == MacroEventType.Pause;
        public string DisplayName
        {
            get
            {
                switch (EventType)
                {
                    case MacroEventType.KeyPress: return "Key Press";
                    case MacroEventType.Pause: return "Pause";
                    case MacroEventType.Type: return "Type";
                }
                return "";
            }
        }

        public MacroEventViewModel()
        {
        }

        public MacroEventViewModel(MacroEvent model)
        {
            EventType = model.EventType;
            Key = model.Key;
            HoldTimeMilliseconds = model.HoldTimeMilliseconds;
            TypedPhrase = model.TypedPhrase;
        }
    }
}
