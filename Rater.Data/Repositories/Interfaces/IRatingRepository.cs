using Rater.Domain.DataTransferObjects.RatingDto;
using Rater.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Data.Repositories.Interfaces
{
    public interface IRatingRepository
    {
        Task<RatingResponseDto> AddRatings(List<RatingModel> request);
        Task<List<RatingModel>> GetRatings(int space_id);
    }
}
