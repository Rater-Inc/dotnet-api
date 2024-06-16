using System.ComponentModel.DataAnnotations;

namespace Rater.Domain.DataTransferObjects.RatingDto
{
    public class RatingDetailDto
    {
        [Required]
        public int RateeId { get; set; }
        [Required]
        public int MetricId { get; set; }
        [Required]
        public int Score { get; set; }
    }
}
    