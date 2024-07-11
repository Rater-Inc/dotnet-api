using Rater.Domain.DataTransferObjects.MetricDto;
using Rater.Domain.DataTransferObjects.ParticipantDto;

namespace Rater.Domain.DataTransferObjects.SpaceDto
{
    public class SpaceResponseDto
    {
        public int SpaceId { get; set; }

        public int CreatorId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public bool? IsLocked { get; set; }

        public string Link { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<MetricResponseDto> Metrics { get; set; } = new List<MetricResponseDto>();
        public virtual ICollection<ParticipantResponseDto> Participants { get; set; } = new List<ParticipantResponseDto>();



    }
}
