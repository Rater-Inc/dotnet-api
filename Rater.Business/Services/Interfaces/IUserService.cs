

using Rater.API;
using Rater.Domain.DataTransferObjects.UserDto;

namespace Rater.Business.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDto> CreateUser(UserRequestDto request);
    }
}
