﻿using Rater.Business.Services.Interfaces;
using Rater.Data.Repositories.Interfaces;
using Rater.Domain.DataTransferObjects.UserDto;

namespace Rater.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }


        public async Task<UserResponseDto> CreateUser(UserRequestDto request)
        {
            var value = await _repo.AddUser(request);
            return value;
        }
    }
}
