using Rater.Domain.DataTransferObjects.MetricDto;
using Rater.Domain.DataTransferObjects.ParticipantDto;

namespace Rater.Domain.DataTransferObjects.SpaceDto
{
    public class GrandSpaceRequestDto
    {
        public string creatorNickname { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool IsLocked { get; set; } = false;

        public string Password { get; set; } = string.Empty;

        public virtual ICollection<MetricRequestDto> Metrics { get; set; } = new List<MetricRequestDto>();
        public virtual ICollection<ParticipantRequestDto> Participants { get; set; } = new List<ParticipantRequestDto>();
    }
}
