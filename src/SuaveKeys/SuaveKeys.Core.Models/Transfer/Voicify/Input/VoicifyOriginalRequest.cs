using System;
using System.Collections.Generic;
using System.Text;

namespace SuaveKeys.Core.Models.Transfer.Voicify.Input
{
    public class VoicifyOriginalRequest
    {
        public string RequestType { get; set; }
        public string Assistant { get; set; }
        public string SessionId { get; set; }
        public string UserId { get; set; }
        public string ApplicationId { get; set; }
        public Dictionary<string,string> Slots { get; set; }
        public Dictionary<string, object> SessionAttributes { get; set; }
        public object NativeRequest { get; set; }
        public string AccessToken { get; set; }
    }
}
