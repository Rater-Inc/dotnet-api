namespace Rater.Domain.Models;

public partial class SpaceModel : BaseModel
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool? IsLocked { get; set; }
    public string? Password { get; set; }
    public string Link { get; set; } = null!;


    public int CreatorId { get; set; }
    public virtual UserModel Creator { get; set; } = null!;

    public virtual ICollection<MetricModel> Metrics { get; set; } = [];
    public virtual ICollection<ParticipantModel> Participants { get; set; } = [];
    public virtual ICollection<RatingModel> Ratings { get; set; } = [];
}
