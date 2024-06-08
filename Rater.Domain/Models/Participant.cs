using System;
using System.Collections.Generic;

namespace Rater.API;

public partial class Participant
{
    public int ParticipantId { get; set; }

    public int SpaceId { get; set; }

    public string ParticipantName { get; set; } = null!;

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual Space Space { get; set; } = null!;
}
