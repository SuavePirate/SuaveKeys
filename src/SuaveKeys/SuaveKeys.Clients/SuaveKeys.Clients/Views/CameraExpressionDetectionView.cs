using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace SuaveKeys.Clients.Views
{
    /// <summary>
    /// This is only for Windows (maybe mac?), so don't use renderers in the other platforms
    /// </summary>
    public class CameraExpressionDetectionView : View
    {
        public static readonly BindableProperty CameraProperty = BindableProperty.Create(
            propertyName: "Camera",
            returnType: typeof(CameraOptions),
            declaringType: typeof(CameraExpressionDetectionView),
            defaultValue: CameraOptions.Default);
        public event EventHandler<Stream> OnFrameStreamProcess;
        public CameraOptions Camera
        {
            get { return (CameraOptions)GetValue(CameraProperty); }
            set { SetValue(CameraProperty, value); }
        }

        public void ProcessFrameStream(MemoryStream memoryStream)
        {
            OnFrameStreamProcess?.Invoke(this, memoryStream);
        }
    }
}
