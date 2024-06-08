using System;
using System.Collections.Generic;

namespace Rater.API;

public partial class User
{
    public int UserId { get; set; }

    public string Nickname { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<Space> Spaces { get; set; } = new List<Space>();
}
