using Rater.Domain.DataTransferObjects.AuthDto;

namespace Rater.Business.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> AuthLobbyAsync(string link, string password);
        string CreateToken(int space_id);
        Task ValidateAuthorizationAsync(int spaceId);
    }
}
