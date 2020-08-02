using SuaveKeys.Clients.Services;
using SuaveKeys.Clients.UWP.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TinyIoC;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Xamarin.Forms;

namespace SuaveKeys.Clients.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            var container = TinyIoCContainer.Current;
            container.Register<IAuthClientSettings, UwpAuthClientSettings>();
            container.Register<IKeyboardService, ArduinoSerialKeyboardService>();
            var app = new SuaveKeys.Clients.App(container);
            LoadApplication(app);
        }
    }
}
