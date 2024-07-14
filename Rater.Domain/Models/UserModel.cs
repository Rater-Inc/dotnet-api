namespace Rater.Domain.Models;

public partial class UserModel : BaseModel
{
    public string Nickname { get; set; } = null!;

    public virtual ICollection<RatingModel> Ratings { get; set; } = [];
    public virtual ICollection<SpaceModel> Spaces { get; set; } = [];
}
