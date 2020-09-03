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
    public class AndroidKeyboardService : IKeyboardService
    {
        public event EventHandler<KeyboardEventArgs> OnKeyEvent;

        public Task Press(string key)
        {
            return Task.CompletedTask; // NOTE: we have not implemented the custom keyboard on android
        }

        public Task Type(string input)
        {
            return Task.CompletedTask; // NOTE: we have not implemented the custom keyboard on android
        }
    }
}