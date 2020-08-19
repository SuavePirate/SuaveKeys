using Acr.UserDialogs;
using SuaveKeys.Clients.Services;
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
    public class MacroPageViewModel : BaseViewModel
    {
        private readonly IKeyboardProfileService _keyboardService;

        public UserKeyboardProfileModel CurrentProfile { get; set; }
        public ICommand SaveProfileCommand { get; set; }
        public ICommand CreateMacroCommand { get; set; }
        public ICommand CreateMacroEventCommand { get; set; }
        public ICommand RemoveEventCommand { get; set; }
        public ICommand MoveEventUpCommand { get; set; }
        public ICommand MoveEventDownCommand { get; set; }
        public string CurrentKey { get; set; }
        public MacroModel CurrentMacro { get; set; }
        public ObservableCollection<MacroModel> Macros { get; set; }
        public string SelectedNewEventType { get; set; }
        public ObservableCollection<MacroEventViewModel> CurrentEvents { get; set; }
        public bool HasCurrentMacro => CurrentMacro != null;
        public MacroPageViewModel()
        {
            _keyboardService = App.Current.Container.Resolve<IKeyboardProfileService>();
            CreateMacroCommand = new Command(async () =>
            {
                var name = await UserDialogs.Instance.PromptAsync("Enter a new macro phrase. This is the spoken phrase to trigger the macro", "New Macro", "Ok", "Cancel", "Spoken phrase for macro");
                if (!string.IsNullOrEmpty(name?.Text))
                {
                    Macros.Add(new MacroModel
                    {
                        Phrase = name.Text,
                        Events = new List<MacroEvent>()
                    });
                }
            });
            CreateMacroEventCommand = new Command(() =>
            {
                if (CurrentMacro == null)
                    return;

                var type = MacroEventType.KeyPress;
                switch(SelectedNewEventType)
                {
                    case "Pause": type = MacroEventType.Pause;
                        break;
                    case "Type": type = MacroEventType.Type;
                        break;
                }
                CurrentEvents.Add(new MacroEventViewModel
                {
                    EventType = type
                });
            });
            RemoveEventCommand = new Command<MacroEventViewModel>((eventModel) =>
            {
                CurrentEvents?.Remove(eventModel);
            });

            MoveEventUpCommand = new Command<MacroEventViewModel>((eventModel) =>
            {
                var currentIndex = CurrentEvents.IndexOf(eventModel);
                if (currentIndex == 0)
                    return;

                CurrentEvents.Move(currentIndex, currentIndex - 1);
            });
            MoveEventDownCommand = new Command<MacroEventViewModel>((eventModel) =>
            {
                var currentIndex = CurrentEvents.IndexOf(eventModel);
                if (currentIndex == CurrentEvents.Count - 1)
                    return;

                CurrentEvents.Move(currentIndex, currentIndex + 1);
            });
            SaveProfileCommand = new Command(async () =>
            {
                if (CurrentProfile?.Configuration == null)
                    return;

                var macros = Macros?.ToList() ?? new List<MacroModel>();
                var currentMacro = macros.FirstOrDefault(m => m.Phrase == CurrentMacro.Phrase);
                if (currentMacro == null)
                    return;

                currentMacro.Events = CurrentEvents.Select(m => m as MacroEvent).ToList();

                // make API call
                await _keyboardService.UpdateProfile(CurrentProfile.Id, CurrentProfile.Name, new KeyboardProfileConfiguration()
                {
                    CommandKeyMappings = CurrentProfile?.Configuration?.CommandKeyMappings ?? new Dictionary<string, string>(),
                    Macros = macros
                });

            });
            PropertyChanged += ProfilePageViewModel_PropertyChanged;

        }

        private void ProfilePageViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
           if(e.PropertyName == nameof(CurrentMacro))
            {
                CurrentEvents = new ObservableCollection<MacroEventViewModel>(
                    CurrentMacro?.Events?.Select(m => new MacroEventViewModel(m)) ?? new List<MacroEventViewModel>());
            }
        }

    }
}
