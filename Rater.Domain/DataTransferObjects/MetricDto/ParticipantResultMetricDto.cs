

namespace Rater.Domain.DataTransferObjects.MetricDto
{
    public class ParticipantResultMetricDto
    {
        public int MetricId { get; set; }
        public string Name { get; set; } = null!;
        public double averageMetricScore { get; set; }

    }
}
