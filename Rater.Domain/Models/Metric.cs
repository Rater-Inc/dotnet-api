using System;
using System.Collections.Generic;

namespace Rater.API;

public partial class Metric
{
    public int MetricId { get; set; }

    public int SpaceId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual Space Space { get; set; } = null!;
}
