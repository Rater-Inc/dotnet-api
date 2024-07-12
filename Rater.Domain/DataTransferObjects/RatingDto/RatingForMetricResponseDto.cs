using Rater.Domain.DataTransferObjects.ParticipantDto;
using Rater.Domain.DataTransferObjects.UserDto;

namespace Rater.Domain.DataTransferObjects.RatingDto
{
    public class RatingForMetricResponseDto
    {
        public int RatingId { get; set; }

        public int RaterId { get; set; }

        public int RateeId { get; set; }

        public int Score { get; set; }

        public DateTime? RatedAt { get; set; }

        public ParticipantResponseDto Ratee { get; set; } = null!;
        public UserResponseDto Rater { get; set; } = null!;
    }
}
