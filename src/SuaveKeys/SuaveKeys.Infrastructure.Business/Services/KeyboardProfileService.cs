using Newtonsoft.Json;
using ServiceResult;
using SuaveKeys.Core.Business.Services;
using SuaveKeys.Core.Data.Repositories;
using SuaveKeys.Core.Models.Entities;
using SuaveKeys.Core.Models.Transfer.Keyboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuaveKeys.Infrastructure.Business.Services
{
    public class KeyboardProfileService : IKeyboardProfileService
    {
        private readonly IUserKeyboardProfileRepository _profileRepository;

        public KeyboardProfileService(IUserKeyboardProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public async Task<Result<UserKeyboardProfileModel>> CreateNewConfiguration(string userId, string name, KeyboardProfileConfiguration configuration)
        {
            try
            {
                var entity = new UserKeyboardProfile
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    Name = name,
                    ConfigurationJson = JsonConvert.SerializeObject(configuration)
                };

                await _profileRepository.Add(entity);
                await _profileRepository.SaveChangesAsync();

                return new SuccessResult<UserKeyboardProfileModel>(new UserKeyboardProfileModel(entity));
            }
            catch(Exception)
            {
                return new UnexpectedResult<UserKeyboardProfileModel>();
            }
        }

        public async Task<Result<UserKeyboardProfileModel>> DeleteConfiguration(string userId, string profileId)
        {
            try
            {
                var profile = await _profileRepository.FindById(profileId);
                if (profile is null)
                    return new InvalidResult<UserKeyboardProfileModel>("Invalid profile ID.");

                if (profile.UserId != userId)
                    return new InvalidResult<UserKeyboardProfileModel>("You cannot delete a profile you don't own.");

                await _profileRepository.Remove(profile);
                await _profileRepository.SaveChangesAsync();

                return new SuccessResult<UserKeyboardProfileModel>(new UserKeyboardProfileModel(profile));
            }
            catch (Exception)
            {
                return new UnexpectedResult<UserKeyboardProfileModel>();
            }
        }

        public async Task<Result<List<UserKeyboardProfileModel>>> GetConfigurationsForUser(string userId)
        {
            try
            {
                var profiles = await _profileRepository.GetForUser(userId);
                return new SuccessResult<List<UserKeyboardProfileModel>>(profiles?.Select(p => new UserKeyboardProfileModel(p))?.ToList() ?? new List<UserKeyboardProfileModel>());
            }
            catch (Exception)
            {
                return new UnexpectedResult<List<UserKeyboardProfileModel>>();
            }
        }

        public async Task<Result<UserKeyboardProfileModel>> UpdateConfiguration(string userId, string profileId, string name, KeyboardProfileConfiguration configuration)
        {
            try
            {
                var profile = await _profileRepository.FindById(profileId);
                if (profile is null)
                    return new InvalidResult<UserKeyboardProfileModel>("Invalid profile ID.");

                if (profile.UserId != userId)
                    return new InvalidResult<UserKeyboardProfileModel>("You cannot delete a profile you don't own.");

                profile.Name = name;
                profile.ConfigurationJson = JsonConvert.SerializeObject(configuration);

                await _profileRepository.SaveChangesAsync();

                return new SuccessResult<UserKeyboardProfileModel>(new UserKeyboardProfileModel(profile));

            }
            catch (Exception)
            {
                return new UnexpectedResult<UserKeyboardProfileModel>();
            }
        }
    }
}
