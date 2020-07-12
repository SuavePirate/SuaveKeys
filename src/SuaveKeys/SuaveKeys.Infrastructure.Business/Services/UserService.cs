using SuaveKeys.Core.Business.Services;
using SuaveKeys.Core.Data.Providers;
using SuaveKeys.Core.Data.Repositories;
using SuaveKeys.Core.Models.Entities;
using SuaveKeys.Core.Models.Transfer.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SuaveKeys.Infrastructure.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashProvider _hashProvider;

        public UserService(IUserRepository userRepository, IHashProvider hashProvider)
        {
            _userRepository = userRepository;
            _hashProvider = hashProvider;
        }

        public async Task<UserModel> CreateUser(NewUserRequest request)
        {
            var entity = new User
            {
                Email = request.Email,
                PasswordHash = _hashProvider.HashPassword(request.Password)
            };

            entity = await _userRepository.Add(entity);
            await _userRepository.SaveChangesAsync();

            return new UserModel
            {
                Email = entity.Email
            };
        }
    }
}
