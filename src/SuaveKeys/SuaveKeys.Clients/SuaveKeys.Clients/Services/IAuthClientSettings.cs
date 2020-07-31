using System;
using System.Collections.Generic;
using System.Text;

namespace SuaveKeys.Clients.Services
{
    public interface IAuthClientSettings
    {
        string ClientId { get; }
        string ClientSecret { get; }
    }
}
