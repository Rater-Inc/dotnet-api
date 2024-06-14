using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Domain.DataTransferObjects.RatingDto
{
    public class RatingDetailDto
    {

        public int RateeId { get; set; }

        public int MetricId { get; set; }

        public int Score { get; set; }
    }
}
    