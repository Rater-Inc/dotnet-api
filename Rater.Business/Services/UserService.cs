using AutoMapper;
using Rater.Business.Services.Interfaces;
using Rater.Data.Repositories.UserRepositories;
using Rater.Domain.DataTransferObjects.UserDto;
using Rater.Domain.Models;

namespace Rater.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repo, IMapper mapper)
        {
            _userRepository = repo;
            _mapper = mapper;
        }

        public async Task<UserResponseDto> CreateUserAsync(UserRequestDto request)
        {
            var user = _mapper.Map<UserModel>(request);

            var response = await _userRepository.CreateAsync(user);

            return _mapper.Map<UserResponseDto>(response);
        }
    }
}
