using Rater.Domain.DataTransferObjects.MetricDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Domain.DataTransferObjects.ParticipantDto
{
    public class ParticipantWinnerDto
    {
        public int ParticipantId { get; set; }
        public string ParticipantName { get; set; } = null!;
        public double AverageScore { get; set; }

        public virtual ICollection<MetricOfParticipantWinnerDto> Metrics { get; set; } = new List<MetricOfParticipantWinnerDto>();

    }
}
