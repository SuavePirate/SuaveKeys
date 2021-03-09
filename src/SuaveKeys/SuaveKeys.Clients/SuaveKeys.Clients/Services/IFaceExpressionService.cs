using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SuaveKeys.Clients.Services
{
    public interface IFaceExpressionService
    {
        /// <summary>
        /// Returns the parsed expression from a video frame
        /// </summary>
        /// <param name="frameStream">A memory stream of a frame of a video to process for facial expression</param>
        /// <returns>a string identifier of the detected expression or null if none found</returns>
        Task<string> GetExpressionMarkerAsync(Stream frameStream);
    }
}
