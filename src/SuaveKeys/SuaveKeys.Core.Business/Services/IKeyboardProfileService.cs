using ServiceResult;
using SuaveKeys.Core.Models.Transfer.Keyboard;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SuaveKeys.Core.Business.Services
{
    public interface IKeyboardProfileService
    {
        Task<Result<UserKeyboardProfileModel>> CreateNewConfiguration(string userId, string name, KeyboardProfileConfiguration configuration);
        Task<Result<UserKeyboardProfileModel>> UpdateConfiguration(string userId, string profileId, string name, KeyboardProfileConfiguration configuration);
        Task<Result<List<UserKeyboardProfileModel>>> GetConfigurationsForUser(string userId);
        Task<Result<UserKeyboardProfileModel>> DeleteConfiguration(string userId, string profileId);
    }
}
