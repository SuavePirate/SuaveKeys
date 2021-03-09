using Microsoft.Azure.CognitiveServices.Vision.Face;
using SuaveKeys.Clients.Models.CogServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SuaveKeys.Clients.Services
{
    public class CognitiveServicesFaceExpressionService : IFaceExpressionService
    {
        private readonly IFaceClient _faceClient;
        public CognitiveServicesFaceExpressionService()
        {
            _faceClient = new FaceClient(new ApiKeyServiceClientCredentials(CogServicesKeys.SubscriptionKey)) { Endpoint = CogServicesKeys.Endpoint };
        }
        public async Task<string> GetExpressionMarkerAsync(Stream frameStream)
        {
            try
            {
                var result = await _faceClient.Face.DetectWithStreamAsync(frameStream, true, true);
                foreach(var face in result)
                {
                    Console.WriteLine(face);
                }

                // TODO: handle the expressions we can check
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
