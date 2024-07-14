namespace Rater.Domain.Models;

public abstract class BaseModel
{
    public int Id { get; set; }

    public DateTime CreatedDate { get; set; }
}
