using Rater.API;
using Rater.Domain.DataTransferObjects.RatingDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Domain.DataTransferObjects.MetricDto
{
    public class MetricResponseDto
    {
        public int MetricId { get; set; }

        public int SpaceId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public virtual ICollection<RatingForMetricResponseDto> Ratings { get; set; } = new List<RatingForMetricResponseDto>();

    }
}
