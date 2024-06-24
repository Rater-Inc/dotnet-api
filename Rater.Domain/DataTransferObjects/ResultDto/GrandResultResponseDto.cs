using Rater.API;
using Rater.Domain.DataTransferObjects.MetricDto;
using Rater.Domain.DataTransferObjects.ParticipantDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Domain.DataTransferObjects.ResultDto
{
    public class GrandResultResponseDto
    {
        public int SpaceId { get; set; }

        public string? Name { get; set; }

        public virtual ICollection<ParticipantWinnerDto> ParticipantResults { get; set; } = new List<ParticipantWinnerDto>();

        public virtual ICollection<MetricWinnerDto> MetricWinners { get; set; } = new List<MetricWinnerDto>();
        
    }
}
