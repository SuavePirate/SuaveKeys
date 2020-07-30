using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SuaveKeys.Clients.Services
{
    /// <summary>
    /// Responsible for typing/pressing keys on the native keyboard level
    /// </summary>
    public interface IKeyboardService
    {
        Task Type(string input);
        Task Press(string key);
    }
}
