using SuaveKeys.Clients.Services;
using SuaveKeys.Clients.Uwp;
using SuaveKeys.Clients.UWP.Views;
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
        public async Task Press(string key)
        {
            if (EventHandlerForDevice.Current.IsDeviceConnected)
            {
                var dataWriter = DeviceSelectorView.Current?.DataWriterObject;
                try
                {
                    dataWriter = new DataWriter(EventHandlerForDevice.Current.Device.OutputStream);
                    dataWriter.WriteString($"press {key}\n");
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
                    dataWriter = null;
                }
            }
            else
            {
                Debugger.Break();
            }
        }

        public async Task Type(string input)
        {
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
