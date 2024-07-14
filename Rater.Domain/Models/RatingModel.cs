namespace Rater.Domain.Models;

public partial class RatingModel : BaseModel
{
    public int Score { get; set; }
    public DateTime? RatedAt { get; set; }

    public int MetricId { get; set; }
    public virtual MetricModel Metric { get; set; } = null!;
    public int RateeId { get; set; }
    public virtual ParticipantModel Ratee { get; set; } = null!;

    public int RaterId { get; set; }
    public virtual UserModel Rater { get; set; } = null!;
    public int SpaceId { get; set; }
    public virtual SpaceModel Space { get; set; } = null!;
}
