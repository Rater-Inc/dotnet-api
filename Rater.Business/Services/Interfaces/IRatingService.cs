using Rater.Domain.DataTransferObjects.RatingDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Business.Services.Interfaces
{
    public interface IRatingService
    {
        Task<List<RatingForMetricResponseDto>> AddRatings(RatingRequestDto request);
    }
}
