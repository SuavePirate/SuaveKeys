using SuaveKeys.Clients.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SuaveKeys.Clients.ViewModels
{
    public class CameraPageViewModel : BaseViewModel
    {
        private readonly IFaceExpressionService _faceExpressionService;
        private readonly IKeyboardService _keyboardService;
        public CameraPageViewModel()
        {
            _faceExpressionService = App.Current.Container.Resolve<IFaceExpressionService>();
            _keyboardService = App.Current.Container.Resolve<IKeyboardService>();
        }

        public async Task ProcessFrame(byte[] frame)
        {
            using (var frameStream = new MemoryStream(frame))
            {
                // send the frame to process expression. If we get an expression, pass it as a key press!
                var expression = await _faceExpressionService.GetExpressionMarkerAsync(frameStream);
                if(expression != null)
                {
                    Console.WriteLine(expression);
                    await _keyboardService.Press(expression);
                }
            }
        }
    }
}
