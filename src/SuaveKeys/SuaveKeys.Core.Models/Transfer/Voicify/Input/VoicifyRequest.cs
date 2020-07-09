using System;
using System.Collections.Generic;
using System.Text;

namespace SuaveKeys.Core.Models.Transfer.Voicify.Input
{
    public class VoicifyRequest
    {
        public string Id { get; set; }
        public string RequestNames { get; set; }
        public string Title { get; set; }
        public string ApplicationId { get; set; }
        public VoicifyOriginalRequest OriginalRequest { get; set; }
    }
}

