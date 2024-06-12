using Rater.API;
using Rater.Domain.DataTransferObjects.UserDto;

namespace Rater.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<UserResponseDto> AddUser(UserRequestDto request);
        Task<int> GetCreatorId(DateTime? dateTime);
    }
}
