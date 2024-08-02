

namespace Rater.Domain.DataTransferObjects.MetricDto
{
    public class MetricRequestDto
    {

        public int SpaceId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }
    }
}
