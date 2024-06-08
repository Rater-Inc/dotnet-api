using Rater.API;
using Rater.Business.Services.Interfaces;
using Rater.Data.Repositories.Interfaces;
using Rater.Domain.DataTransferObjects.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
