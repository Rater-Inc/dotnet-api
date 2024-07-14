namespace Rater.Domain.Models;

public partial class ParticipantModel : BaseModel
{
    public string ParticipantName { get; set; } = null!;

    public int SpaceId { get; set; }
    public virtual SpaceModel Space { get; set; } = null!;

    public virtual ICollection<RatingModel> Ratings { get; set; } = [];
}
