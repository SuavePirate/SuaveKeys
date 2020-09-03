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
using Voicify.Sdk.Assistant.Api;
using Voicify.Sdk.Core.Models.Model;

namespace SuaveKeys.Clients.Droid.Services
{
    public class AndroidSpeechToTextService : ISpeechToTextService
    {
        private readonly MainActivity _context;
        private readonly ILanguageService _languageService;
        private readonly ICustomAssistantApi _customAssistantApi;
        private readonly IAuthService _authService;
        private string sessionId;
        public event EventHandler<SpeechRecognizedEventArgs> OnSpeechRecognized;
        public AndroidSpeechToTextService(MainActivity context,
            ILanguageService languageService,
            ICustomAssistantApi customAssistantApi,
            IAuthService authService)
        {
            _context = context;
            _languageService = languageService;
            _customAssistantApi = customAssistantApi;
            _authService = authService;
            _context.OnSpeechRecognized += Context_OnSpeechRecognized;
        }

        private async void Context_OnSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            var languageResult = await _languageService.ProcessLanguage(e.Speech).ConfigureAwait(false);
            var updatedSlots = languageResult.Data.Slots.ToDictionary(s => GetVoicifySlotName(languageResult.Data.Name, s.Name), s => s.Value);
            var tokenResult = await _authService.GetCurrentAccessToken();
            updatedSlots.Add("AccessToken", tokenResult?.Data);
            var voicifyResponse = await _customAssistantApi.HandleRequestAsync(VoicifyKeys.ApplicationId, VoicifyKeys.ApplicationSecret, new CustomAssistantRequestBody(
                    requestId: Guid.NewGuid().ToString(),
                    context: new CustomAssistantRequestContext(sessionId,
                    noTracking: false,
                    requestType: "IntentRequest",
                    requestName: languageResult.Data.Name,
                    slots: updatedSlots,
                    originalInput: e.Speech,
                    channel: "Android App",
                    requiresLanguageUnderstanding: false,
                    locale: "en-us"),
                    new CustomAssistantDevice(Guid.NewGuid().ToString(), "Android Device"),
                    new CustomAssistantUser(sessionId, "Android User")
                ));

            OnSpeechRecognized?.Invoke(this, e);
        }

        private string GetVoicifySlotName(string intentName, string nativeSlotName)
        {
            if (intentName == "PressKeyIntent" && nativeSlotName == "wit$search_query")
                return "key";
            if (intentName == "TypeIntent" && nativeSlotName == "wit$search_query")
                return "phrase";
            if (intentName == "VoicifyLatestMessageIntent" && nativeSlotName == "wit$search_query")
                return "category";
            return "query";
        }

        public Task InitializeAsync()
        {
            sessionId = Guid.NewGuid().ToString();
            // we don't need to init.
            return Task.CompletedTask;
        }

        public Task StartAsync()
        {
            var voiceIntent = new Android.Content.Intent(RecognizerIntent.ActionRecognizeSpeech);
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