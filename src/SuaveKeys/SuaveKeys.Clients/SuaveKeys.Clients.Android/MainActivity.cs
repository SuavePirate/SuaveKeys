using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Acr.UserDialogs;
using SuaveKeys.Clients.Droid.Services;
using SuaveKeys.Clients.Services;
using Xamarin.Forms;
using TinyIoC;
using SuaveKeys.Clients.Models;
using Android.Content;
using Android.Speech;
using Voicify.Sdk.Assistant.Api;

namespace SuaveKeys.Clients.Droid
{
    [Activity(Label = "SuaveKeys.Clients", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public event EventHandler<SpeechRecognizedEventArgs> OnSpeechRecognized;
        public const int VOICE_RESULT = 100;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            UserDialogs.Init(this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            var container = TinyIoCContainer.Current;
            container.Register<IAuthClientSettings, AndroidAuthClientSettings>();
            container.Register<IKeyboardService, AndroidKeyboardService>();
            container.Register<ILanguageService, WitLanguageUnderstandingService>(); // NOTE: if we abstract this to be used in the other platforms, move to core app regs
            container.Register<ISpeechToTextService>((c, o) =>
            {
                return new AndroidSpeechToTextService(this, c.Resolve<ILanguageService>(), new CustomAssistantApi("https://assistant.voicify.com"), c.Resolve<IAuthService>());
            });


            LoadApplication(new App(container));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


        protected override void OnActivityResult(int requestCode, Result resultVal, Intent data)
        {
            if (requestCode == VOICE_RESULT)
            {
                if (resultVal == Result.Ok)
                {
                    var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                    if (matches.Count != 0)
                    {
                        OnSpeechRecognized?.Invoke(this, new SpeechRecognizedEventArgs
                        {
                            Speech = matches[0]
                        });
                        //string textInput = textBox.Text + matches[0];
                        //textBox.Text = textInput;
                        //switch (matches[0].Substring(0, 5).ToLower())
                        //{
                        //    case "north":
                        //        MovePlayer(0);
                        //        break;
                        //    case "south":
                        //        MovePlayer(1);
                        //        break;
                        //}
                    }
                    else
                    {
                        //textBox.Text = "No speech was recognised";
                    }
                }
                base.OnActivityResult(requestCode, resultVal, data);
            }
        }
    }
}