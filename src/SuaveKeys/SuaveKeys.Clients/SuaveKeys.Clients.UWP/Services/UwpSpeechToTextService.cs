using SuaveKeys.Clients.Models;
using SuaveKeys.Clients.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.ExtendedExecution;
using Windows.Media.Capture;
using Windows.Media.SpeechRecognition;

namespace SuaveKeys.Clients.UWP.Services
{
    public class UwpSpeechToTextService : ISpeechToTextService
    {
        public event EventHandler<SpeechRecognizedEventArgs> OnSpeechRecognized;

        public async Task StartAsync()
        {
            var hasPermission = await AudioCapturePermissions.RequestMicrophonePermission();
            if (!hasPermission) return;
            var session = new ExtendedExecutionSession();
            session.Reason = ExtendedExecutionReason.Unspecified;
            var result = await session.RequestExtensionAsync();
            if (result == ExtendedExecutionResult.Allowed)
            {
                // Create an instance of SpeechRecognizer.
                var speechRecognizer = new SpeechRecognizer();

                // Compile the dictation grammar by default.
                await speechRecognizer.CompileConstraintsAsync();

                // Start recognition.
                var speechRecognitionResult = await speechRecognizer.RecognizeAsync();

                // Do something with the recognition result.
                OnSpeechRecognized?.Invoke(this, new SpeechRecognizedEventArgs
                {
                    Speech = speechRecognitionResult.Text
                });
            }
        }

        private class AudioCapturePermissions
        {
            // If no microphone is present, an exception is thrown with the following HResult value.
            private static int NoCaptureDevicesHResult = -1072845856;

            /// <summary>
            /// Note that this method only checks the Settings->Privacy->Microphone setting, it does not handle
            /// the Cortana/Dictation privacy check.
            ///
            /// You should perform this check every time the app gets focus, in case the user has changed
            /// the setting while the app was suspended or not in focus.
            /// </summary>
            /// <returns>True, if the microphone is available.</returns>
            public async static Task<bool> RequestMicrophonePermission()
            {
                try
                {
                    // Request access to the audio capture device.
                    var settings = new MediaCaptureInitializationSettings();
                    settings.StreamingCaptureMode = StreamingCaptureMode.Audio;
                    settings.MediaCategory = MediaCategory.Speech;
                    var capture = new MediaCapture();

                    await capture.InitializeAsync(settings);
                }
                catch (TypeLoadException)
                {
                    // Thrown when a media player is not available.
                    var messageDialog = new Windows.UI.Popups.MessageDialog("Media player components are unavailable.");
                    await messageDialog.ShowAsync();
                    return false;
                }
                catch (UnauthorizedAccessException)
                {
                    // Thrown when permission to use the audio capture device is denied.
                    // If this occurs, show an error or disable recognition functionality.
                    return false;
                }
                catch (Exception exception)
                {
                    // Thrown when an audio capture device is not present.
                    if (exception.HResult == NoCaptureDevicesHResult)
                    {
                        var messageDialog = new Windows.UI.Popups.MessageDialog("No Audio Capture devices are present on this system.");
                        await messageDialog.ShowAsync();
                        return false;
                    }
                    else
                    {
                        throw;
                    }
                }
                return true;
            }
        }
    }
}
