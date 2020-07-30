using SuaveKeys.Clients.UWP.Renderer;
using SuaveKeys.Clients.UWP.Views;
using SuaveKeys.Clients.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(DeviceSelector), typeof(DeviceSelectorRenderer))]

namespace SuaveKeys.Clients.UWP.Renderer
{
    public class DeviceSelectorRenderer : ViewRenderer<DeviceSelector, DeviceSelectorView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<DeviceSelector> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                if (Control == null)
                    SetNativeControl(new DeviceSelectorView());
            }
        }
    }
}
