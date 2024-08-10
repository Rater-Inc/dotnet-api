using Rater.API;
using Rater.Domain.DataTransferObjects.RatingDto;

namespace Rater.Business.Services.Interfaces
{
    public interface IRatingService
    {
        Task<RatingResponseDto> AddRatings(List<Rating> request);
        Task<List<Rating>> GetRatings(int space_id);
    }
}
