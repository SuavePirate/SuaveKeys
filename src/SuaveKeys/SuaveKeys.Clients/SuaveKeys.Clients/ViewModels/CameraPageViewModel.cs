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
        public CameraPageViewModel()
        {
            _faceExpressionService = App.Current.Container.Resolve<IFaceExpressionService>();
        }

        public async Task ProcessFrame(Stream frameStream)
        {
            var expression = await _faceExpressionService.GetExpressionMarkerAsync(frameStream);
            Console.WriteLine(expression);
        }
    }
}
