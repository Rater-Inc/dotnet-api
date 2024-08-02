using System.ComponentModel.DataAnnotations;


namespace Rater.Domain.DataTransferObjects.RatingDto
{
    public class RatingRequestDto
    {
        [Required]
        public string RaterNickName { get; set; } = null!;
        [Required]
        public int SpaceId { get; set; }
        [Required]
        public List<RatingDetailDto>? RatingDetails { get; set; }
    }
}
