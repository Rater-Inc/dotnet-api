using Rater.Domain.DataTransferObjects.UserDto;

namespace Rater.Business.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDto> CreateUserAsync(UserRequestDto request);
    }
}
