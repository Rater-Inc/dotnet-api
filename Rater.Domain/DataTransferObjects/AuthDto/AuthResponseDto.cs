
namespace Rater.Domain.DataTransferObjects.AuthDto
{
    public class AuthResponseDto
    {
        public bool Success { get; set; } = false;
        public int spaceId { get; set; }
        public string jwtToken { get; set; } = string.Empty;
    }
}
