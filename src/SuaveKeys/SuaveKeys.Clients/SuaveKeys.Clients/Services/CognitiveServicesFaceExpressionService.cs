using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
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
        private readonly List<FaceAttributeType?> faceAttributes = new List<FaceAttributeType?>
        {
            FaceAttributeType.Gender, FaceAttributeType.Age,
            FaceAttributeType.Smile, FaceAttributeType.Emotion,
            FaceAttributeType.Hair, FaceAttributeType.Accessories
        };
        public CognitiveServicesFaceExpressionService()
        {
            _faceClient = new FaceClient(new ApiKeyServiceClientCredentials(CogServicesKeys.SubscriptionKey)) { Endpoint = CogServicesKeys.Endpoint };
        }
        public async Task<string> GetExpressionMarkerAsync(Stream frameStream)
        {
            try
            {
                var result = await _faceClient.Face.DetectWithStreamAsync(frameStream, true, false, faceAttributes);
                foreach (var face in result)
                {
                    if (face.FaceAttributes.Smile > 0.5) return "smile";
                }

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
