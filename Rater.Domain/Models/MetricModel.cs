namespace Rater.Domain.Models;

public partial class MetricModel : BaseModel
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public int SpaceId { get; set; }
    public virtual SpaceModel Space { get; set; } = null!;

    public virtual ICollection<RatingModel> Ratings { get; set; } = [];
}
