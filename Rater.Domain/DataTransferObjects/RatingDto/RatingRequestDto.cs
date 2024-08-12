using System.ComponentModel.DataAnnotations;


namespace Rater.Domain.DataTransferObjects.RatingDto
{
    public class RatingRequestDto
    {
        public string RaterNickName { get; set; } = string.Empty;
        public int SpaceId { get; set; }
        public List<RatingDetailDto> RatingDetails { get; set; } = new List<RatingDetailDto>();
    }
}
