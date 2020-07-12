using SuaveKeys.Core.Models.Transfer.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SuaveKeys.Core.Business.Services
{
    public interface IUserService
    {
        Task<UserModel> CreateUser(NewUserRequest request);
    }
}
