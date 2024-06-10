using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Domain.DataTransferObjects.MetricDto
{
    public class MetricRequestDto
    {

        public int SpaceId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }
    }
}
