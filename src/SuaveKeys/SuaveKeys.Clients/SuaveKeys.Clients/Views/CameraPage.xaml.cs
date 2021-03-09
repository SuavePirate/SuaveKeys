using SuaveKeys.Clients.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SuaveKeys.Clients.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CameraPage : ContentPage
    {
        private readonly CameraPageViewModel _vm = new CameraPageViewModel();
        public CameraPage()
        {
            InitializeComponent();
            BindingContext = _vm;
		}

        private void Button_Clicked(object sender, EventArgs e)
        {
            switch(CameraPreview.Camera)
            {
                case CameraOptions.Back:
                    CameraPreview.Camera = CameraOptions.Default;
                    return;
                case CameraOptions.Default:
                    CameraPreview.Camera = CameraOptions.External;
                    return;
                case CameraOptions.External:
                    CameraPreview.Camera = CameraOptions.Front;
                    return;
                case CameraOptions.Front:
                    CameraPreview.Camera = CameraOptions.Back;
                    return;
            }
        }

        private async void CameraPreview_OnFrameStreamProcess(object sender, System.IO.Stream e)
        {
            await _vm.ProcessFrame(e);
        }
    }
}