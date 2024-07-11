using Rater.Domain.DataTransferObjects.MetricDto;
using Rater.Domain.DataTransferObjects.ParticipantDto;

namespace Rater.Domain.DataTransferObjects.ResultDto
{
    public class GrandResultResponseDto
    {
        public int SpaceId { get; set; }

        public string? Name { get; set; }

        public virtual ICollection<PariticipantResultDto> ParticipantResults { get; set; } = new List<PariticipantResultDto>();

        public virtual ICollection<MetricLeaderDto> MetricLeaders { get; set; } = new List<MetricLeaderDto>();
        
    }
}
