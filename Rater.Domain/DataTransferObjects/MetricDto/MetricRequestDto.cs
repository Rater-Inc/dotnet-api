

namespace Rater.Domain.DataTransferObjects.MetricDto
{
    public class MetricRequestDto
    {

        public int SpaceId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
