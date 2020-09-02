using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using Android.Views;
using Android.Widget;
using SuaveKeys.Clients.Models;
using SuaveKeys.Clients.Services;

namespace SuaveKeys.Clients.Droid.Services
{
    public class AndroidSpeechToTextService : ISpeechToTextService
    {
        private readonly MainActivity _context;

        public event EventHandler<SpeechRecognizedEventArgs> OnSpeechRecognized;
        public AndroidSpeechToTextService(MainActivity context)
        {
            _context = context;
            _context.OnSpeechRecognized += Context_OnSpeechRecognized;
        }

        private void Context_OnSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            // Do something with the speech
            // TODO: send text to wit. then send to voicify
            OnSpeechRecognized?.Invoke(this, e);
        }

        public Task InitializeAsync()
        {
            // we don't need to init.
            return Task.CompletedTask;
        }

        public Task StartAsync()
        {
            var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
            // TODO: use this class as the listener, so we don't need the UI popup.
            _context.StartActivityForResult(voiceIntent, MainActivity.VOICE_RESULT);

            return Task.CompletedTask;
        }
    }
}