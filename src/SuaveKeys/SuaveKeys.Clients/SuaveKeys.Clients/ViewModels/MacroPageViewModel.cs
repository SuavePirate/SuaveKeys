using SuaveKeys.Clients.Services;
using SuaveKeys.Core.Models.Transfer.Keyboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SuaveKeys.Clients.ViewModels
{
    public class MacroPageViewModel : BaseViewModel
    {
        private readonly IKeyboardProfileService _keyboardService;

        public UserKeyboardProfileModel CurrentProfile { get; set; }

        public ICommand SaveProfileCommand { get; set; }

        public string CurrentKey { get; set; }
        public string CurrentKeyCommands { get; set; }

        public MacroPageViewModel()
        {
            _keyboardService = App.Current.Container.Resolve<IKeyboardProfileService>();

            SaveProfileCommand = new Command(async () =>
            {
                if (string.IsNullOrEmpty(CurrentKey) || CurrentProfile?.Configuration == null)
                    return;

                var mapping = CurrentProfile.Configuration.CommandKeyMappings;

                // get the existing values for the current key
                var commandMappings = mapping?.Where(m => m.Value == CurrentKey)?.ToList();

                // remove the existing values
                foreach (var map in commandMappings)
                    mapping.Remove(map.Key);

                // generate new values based off text box
                var values = CurrentKeyCommands.Split(',');
                foreach (var command in values)
                {
                    mapping.Add(command.Trim(), CurrentKey);
                }

                // make API call
                await _keyboardService.UpdateProfile(CurrentProfile.Id, CurrentProfile.Name, new KeyboardProfileConfiguration()
                {
                    CommandKeyMappings = mapping
                });
            });
            PropertyChanged += ProfilePageViewModel_PropertyChanged;

        }

        private void ProfilePageViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CurrentKey))
            {
                // get the commands from the current profile
                var commandMappings = CurrentProfile?.Configuration?.CommandKeyMappings?.Where(m => m.Value == CurrentKey)?.Select(m => m.Key);
                if (commandMappings != null)
                {
                    CurrentKeyCommands = string.Join(",", commandMappings);
                }
            }
           
        }

    }
}
