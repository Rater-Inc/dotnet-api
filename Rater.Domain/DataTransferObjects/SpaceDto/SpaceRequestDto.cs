using Rater.Domain.DataTransferObjects.MetricDto;
using Rater.Domain.DataTransferObjects.ParticipantDto;

namespace Rater.Domain.DataTransferObjects.SpaceDto
{
    public class SpaceRequestDto
    {
        public int CreatorId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public bool? IsLocked { get; set; } = false!;

        public string? Password { get; set; }

        public virtual ICollection<MetricRequestDto> Metrics { get; set; } = new List<MetricRequestDto>();
        public virtual ICollection<ParticipantRequestDto> Participants { get; set; } = new List<ParticipantRequestDto>();
    }
}
