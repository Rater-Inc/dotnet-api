using Rater.API;
using Rater.Domain.DataTransferObjects.RatingDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Data.Repositories.Interfaces
{
    public interface IRatingRepository
    {
        Task<List<RatingForMetricResponseDto>> AddRatings(List<Rating> request);
        Task<List<Rating>> GetRatings(int space_id);
    }
}
