using Acr.UserDialogs;
using Microsoft.AspNetCore.SignalR.Client;
using SuaveKeys.Clients.Services;
using SuaveKeys.Core.Models.Constants;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SuaveKeys.Clients.ViewModels
{
    public class KeyboardPageViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        private readonly IKeyboardService _keyboardService;
        private HubConnection _connection;
        public string ButtonText { get; set; }
        public ICommand ToggleConnectCommand { get; set; }

        public KeyboardPageViewModel()
        {
            _authService = App.Current.Container.Resolve<IAuthService>();
            _keyboardService = App.Current.Container.Resolve<IKeyboardService>();
            ButtonText = "Connect";
            ToggleConnectCommand = new Command(async () =>
            {
                if (_connection == null)
                {
                    _connection = new HubConnectionBuilder()
                   .WithUrl("https://suavekeys-dev.azurewebsites.net/keyboard", options =>
                   {
                       options.AccessTokenProvider = async () =>
                       {
                           var tokenResult = await _authService.GetCurrentAccessToken();
                           return tokenResult.Data;
                       };
                   })
                   .Build();
                    
                    _connection.On<string>(KeyboardEvents.PressKey, async (key) => await _keyboardService?.Press(key));
                    _connection.On<string>(KeyboardEvents.Type, async (keys) => await _keyboardService?.Type(keys));
                    _connection.Reconnected += Connection_Reconnected;
                    _connection.Closed += Connection_Closed;
                    await _connection.StartAsync();
                }
                else
                {
                    await _connection.StopAsync();
                    _connection = null;
                }

            });
        }

        private Task Connection_Closed(Exception arg)
        {
            ButtonText = "Connect";
            return Task.CompletedTask;
        }

        private Task Connection_Reconnected(string arg)
        {
            ButtonText = "Disconnect";

            return Task.CompletedTask;
        }
    }
}
