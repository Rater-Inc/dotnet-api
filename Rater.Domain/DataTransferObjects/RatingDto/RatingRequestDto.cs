using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Domain.DataTransferObjects.RatingDto
{
    public class RatingRequestDto
    {
        public string RaterNickName { get; set; } = null!;

        public int SpaceId { get; set; }

        public List<RatingDetailDto>? RatingDetails { get; set; }
    }
}
