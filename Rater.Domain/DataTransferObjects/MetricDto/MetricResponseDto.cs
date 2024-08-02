

namespace Rater.Domain.DataTransferObjects.MetricDto
{
    public class MetricResponseDto
    {
        public int MetricId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

    }
}
