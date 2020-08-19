using SuaveKeys.Clients.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SuaveKeys.Clients.Services
{
    public interface ISpeechToTextService
    {
        Task InitializeAsync();
        Task StartAsync();
        event EventHandler<SpeechRecognizedEventArgs> OnSpeechRecognized;
    }
}
