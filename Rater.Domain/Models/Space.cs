using System;
using System.Collections.Generic;

namespace Rater.API;

public partial class Space
{
    public int SpaceId { get; set; }

    public int CreatorId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool? IsLocked { get; set; }

    public string? Password { get; set; }

    public string Link { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual User Creator { get; set; } = null!;

    public virtual ICollection<Metric> Metrics { get; set; } = new List<Metric>();

    public virtual ICollection<Participant> Participants { get; set; } = new List<Participant>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
