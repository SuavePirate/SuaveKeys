using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SuaveKeys.Clients.Models;
using SuaveKeys.Clients.Services;

namespace SuaveKeys.Clients.Droid.Services
{
    public class AndroidSpeechToTextService : ISpeechToTextService
    {
        public event EventHandler<SpeechRecognizedEventArgs> OnSpeechRecognized;

        public Task InitializeAsync()
        {
            throw new NotImplementedException();
        }

        public Task StartAsync()
        {
            throw new NotImplementedException();
        }
    }
}