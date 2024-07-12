

namespace Rater.Domain.DataTransferObjects.UserDto
{
    public class UserResponseDto
    {
        public int UserId { get; set; }

        public string Nickname { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }
    }
}
