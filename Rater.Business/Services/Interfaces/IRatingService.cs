using Rater.Domain.DataTransferObjects.RatingDto;
using Rater.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Business.Services.Interfaces
{
    public interface IRatingService
    {
        Task<RatingResponseDto> AddRatings(RatingRequestDto request);
        Task<List<RatingModel>> GetRatings(int space_id);
    }
}
