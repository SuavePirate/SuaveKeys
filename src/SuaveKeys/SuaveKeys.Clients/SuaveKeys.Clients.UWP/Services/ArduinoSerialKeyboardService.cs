using SuaveKeys.Clients.Models;
using SuaveKeys.Clients.Services;
using SuaveKeys.Clients.Uwp;
using SuaveKeys.Clients.UWP.Views;
using SuaveKeys.Core.Models.Transfer.Keyboard;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace SuaveKeys.Clients.UWP.Services
{
    public class ArduinoSerialKeyboardService : IKeyboardService
    {
        private readonly IKeyboardProfileService _keyboardProfileService;

        public event EventHandler<KeyboardEventArgs> OnKeyEvent;
        public ArduinoSerialKeyboardService(IKeyboardProfileService keyboardProfileService)
        {
            _keyboardProfileService = keyboardProfileService;
        }
        public async Task Press(string keyPhrase)
        {
            if (string.IsNullOrEmpty(keyPhrase))
                return;

            if (EventHandlerForDevice.Current.IsDeviceConnected)
            {
                try
                {
                    // check for macros that match phrase
                    // if macro exists, run the macro
                    OnKeyEvent?.Invoke(this, new KeyboardEventArgs { KeyInfo = $"Key: {keyPhrase}" });
                    if (_keyboardProfileService.CurrentRunningProfile?.Configuration?.Macros?.Any(m => m.Phrase.ToLower() == keyPhrase.ToLower()) == true)
                    {
                        // run the macro
                        OnKeyEvent?.Invoke(this, new KeyboardEventArgs { KeyInfo = $"Running macro." });
                        var macro = _keyboardProfileService.CurrentRunningProfile.Configuration.Macros.FirstOrDefault(m => m.Phrase.ToLower() == keyPhrase.ToLower());
                        foreach(var macroEvent in macro?.Events ?? Enumerable.Empty<MacroEvent>())
                        {
                            switch(macroEvent.EventType)
                            {
                                case MacroEventType.KeyPress: await SendPressKeyEvent(macroEvent.Key, macroEvent.HoldTimeMilliseconds > 50 ? macroEvent.HoldTimeMilliseconds : 50);
                                    break;
                                case MacroEventType.Pause: Thread.Sleep(macroEvent.HoldTimeMilliseconds);
                                    break;
                                case MacroEventType.Type: await Type(macroEvent?.TypedPhrase ?? "");
                                    break;
                            }
                        }
                        return;
                    }
                    // if no macro, check commands
                    else if (_keyboardProfileService.CurrentRunningProfile?.Configuration?.CommandKeyMappings?.ContainsKey(keyPhrase) == true)
                    {
                        keyPhrase = _keyboardProfileService.CurrentRunningProfile?.Configuration?.CommandKeyMappings[keyPhrase];
                        OnKeyEvent?.Invoke(this, new KeyboardEventArgs { KeyInfo = $"Mapped Key: {keyPhrase}" });
                    }
                    // if no commands, send the original phrase

                    await SendPressKeyEvent(keyPhrase);
                }
                catch (OperationCanceledException /*exception*/)
                {
                    Debugger.Break();
                }
                catch (Exception exception)
                {
                    Debugger.Break();
                    Debug.WriteLine(exception.Message.ToString());
                }
                finally
                {
                    DeviceSelectorView.Current?.DataWriterObject.DetachStream();
                    DeviceSelectorView.Current?.SetDataWriterObject(null);
                }
            }
            else
            {
                Debugger.Break();
            }
        }

        private async Task SendPressKeyEvent(string keyPhrase, int? holdMilliseconds = null)
        {
            DeviceSelectorView.Current?.SetDataWriterObject(new DataWriter(EventHandlerForDevice.Current.Device.OutputStream));
            DeviceSelectorView.Current?.DataWriterObject.WriteString($"press {keyPhrase} {holdMilliseconds?.ToString() ?? ""}\n");
            await DeviceSelectorView.Current?.WriteAsync(CancellationToken.None);
        }

        public async Task Type(string input)
        {
            OnKeyEvent?.Invoke(this, new KeyboardEventArgs { KeyInfo = $"Typing: {input}" });
            if (EventHandlerForDevice.Current.IsDeviceConnected)
            {
                try
                {
                    DeviceSelectorView.Current?.SetDataWriterObject(new DataWriter(EventHandlerForDevice.Current.Device.OutputStream));
                    DeviceSelectorView.Current?.DataWriterObject.WriteString($"type {input}\n");
                    await DeviceSelectorView.Current?.WriteAsync(CancellationToken.None);
                }
                catch (OperationCanceledException /*exception*/)
                {
                    Debugger.Break();
                }
                catch (Exception exception)
                {
                    Debugger.Break();
                    Debug.WriteLine(exception.Message.ToString());
                }
                finally
                {
                    DeviceSelectorView.Current?.DataWriterObject.DetachStream();
                    DeviceSelectorView.Current?.SetDataWriterObject(null);
                }
            }
            else
            {
                Debugger.Break();
            }
        }
    }
}
