using SuaveKeys.Clients.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SuaveKeys.Clients.ViewModels
{
    public class MicrophonePageViewModel : BaseViewModel
    {
        private readonly ISpeechToTextService _speechToTextService;
        private readonly IKeyboardService _keyboardService;
        public ICommand StartCommand { get; set; }
        public ICommand StopCommand { get; set; }
        public bool IsListening { get; set; }
        public MicrophonePageViewModel()
        {
            _speechToTextService = App.Current.Container.Resolve<ISpeechToTextService>();
            _keyboardService = App.Current.Container.Resolve<IKeyboardService>();

            _speechToTextService.OnSpeechRecognized += SpeechToTextService_OnSpeechRecognized;

            StartCommand = new Command(async () =>
            {
                await _speechToTextService?.InitializeAsync();
                await _speechToTextService?.StartAsync();
                IsListening = true;
            });
            StopCommand = new Command(() =>
            {
                IsListening = false;
            });
        }

        private async void SpeechToTextService_OnSpeechRecognized(object sender, Models.SpeechRecognizedEventArgs e)
        {
            _keyboardService?.Press(e.Speech);
            if (IsListening)
                await _speechToTextService?.StartAsync();
        }
    }
}
