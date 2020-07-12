using System;
using System.Collections.Generic;
using System.Text;

namespace SuaveKeys.Core.Data.Providers
{
    public interface IHashProvider
    {
        string HashPassword(string password);
        bool GetPasswordMatch(string hashedPassword, string password);
    }
}
