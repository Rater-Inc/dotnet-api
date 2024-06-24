using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Domain.DataTransferObjects.MetricDto
{
    public class MetricOfParticipantWinnerDto
    {
        public int MetricId { get; set; }
        public string Name { get; set; } = null!;
        public double averageMetricScore { get; set; }

    }
}
