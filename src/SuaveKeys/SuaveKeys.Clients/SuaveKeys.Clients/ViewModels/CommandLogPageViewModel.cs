using SuaveKeys.Clients.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SuaveKeys.Clients.ViewModels
{
    public class CommandLogPageViewModel : BaseViewModel
    {
        private readonly IKeyboardService _keyboardService;
        public ObservableCollection<string> CommandItems { get; set; }
        public CommandLogPageViewModel()
        {
            CommandItems = new ObservableCollection<string>();
            _keyboardService = App.Current.Container.Resolve<IKeyboardService>();
            _keyboardService.OnKeyEvent += KeyboardService_OnKeyEvent;
        }

        private void KeyboardService_OnKeyEvent(object sender, Models.KeyboardEventArgs e)
        {
            CommandItems.Add(e.KeyInfo);
        }
    }
}
