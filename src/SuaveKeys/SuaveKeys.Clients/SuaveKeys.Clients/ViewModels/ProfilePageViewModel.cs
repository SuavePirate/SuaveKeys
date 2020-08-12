using Acr.UserDialogs;
using SuaveKeys.Clients.Services;
using SuaveKeys.Clients.Views;
using SuaveKeys.Core.Models.Transfer.Keyboard;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SuaveKeys.Clients.ViewModels
{
    public class ProfilePageViewModel : BaseViewModel
    {
        private readonly IKeyboardProfileService _keyboardService;
        public ObservableCollection<UserKeyboardProfileModel> Profiles => _keyboardService.Profiles;
        public UserKeyboardProfileModel CurrentProfile { get; set; }
        public ICommand LoadProfilesCommand { get; set; }
        public ICommand CreateProfileCommand { get; set; }
        public ICommand CommandsCommand { get; set; }
        public ICommand SaveProfileCommand { get; set; }
        public ICommand MacroCommand { get; set; }
        public string CurrentKey { get; set; }
        public string CurrentKeyCommands { get; set; }
        public string CurrentName { get; set; }
        public bool IsProfileSelected => CurrentProfile != null;

        public ProfilePageViewModel()
        {
            _keyboardService = App.Current.Container.Resolve<IKeyboardProfileService>();

            LoadProfilesCommand = new Command(async () =>
            {
                await _keyboardService.LoadProfilesAsync();
            });
            CreateProfileCommand = new Command(async () =>
            {
                var name = await UserDialogs.Instance.PromptAsync("Enter a new profile name", "New Profile", "Ok", "Cancel", "New Profile");
                if(!string.IsNullOrEmpty(name?.Text))
                {
                    await _keyboardService.AddNewProfile(name?.Text, new KeyboardProfileConfiguration()
                    {
                        CommandKeyMappings = new Dictionary<string, string>()
                    });
                }
            }); 
            SaveProfileCommand = new Command(async () =>
            {
                if (CurrentProfile?.Configuration == null)
                    return;

                // make API call
                await _keyboardService.UpdateProfile(CurrentProfile.Id, CurrentName, CurrentProfile.Configuration);
            });
            CommandsCommand = new Command(async () =>
            {
                await (App.Current.MainPage as NavigationPage)?.PushAsync(new KeyboardCommandPage(CurrentProfile));
            });

            MacroCommand = new Command(async () =>
            {
                await (App.Current.MainPage as NavigationPage)?.PushAsync(new MacroPage(CurrentProfile));
            });


            this.PropertyChanged += ProfilePageViewModel_PropertyChanged;
        }

        private void ProfilePageViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(CurrentProfile))
            {
                CurrentName = CurrentProfile?.Name;
                // get the commands from the current profile
                var commandMappings = CurrentProfile?.Configuration?.CommandKeyMappings?.Where(m => m.Value == CurrentKey)?.Select(m => m.Key);
                if (commandMappings != null)
                {
                    CurrentKeyCommands = string.Join(",", commandMappings);
                }

                _keyboardService.CurrentRunningProfile = CurrentProfile;
            }
        }
    }
}
