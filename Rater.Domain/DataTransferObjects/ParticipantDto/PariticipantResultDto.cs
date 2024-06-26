using Rater.Domain.DataTransferObjects.MetricDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Domain.DataTransferObjects.ParticipantDto
{
    public class PariticipantResultDto
    {
        public int ParticipantId { get; set; }
        public string ParticipantName { get; set; } = null!;
        public double AverageScore { get; set; }

        public List<ParticipantResultMetricDto> MetricResults { get; set; } = new List<ParticipantResultMetricDto>();

    }
}
