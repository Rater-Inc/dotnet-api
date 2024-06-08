using System;
using System.Collections.Generic;

namespace Rater.API;

public partial class Rating
{
    public int RatingId { get; set; }

    public int RaterId { get; set; }

    public int RateeId { get; set; }

    public int SpaceId { get; set; }

    public int MetricId { get; set; }

    public int Score { get; set; }

    public DateTime? RatedAt { get; set; }

    public virtual Metric Metric { get; set; } = null!;

    public virtual Participant Ratee { get; set; } = null!;

    public virtual User Rater { get; set; } = null!;

    public virtual Space Space { get; set; } = null!;
}
