using Rater.Domain.DataTransferObjects.RatingDto;
using Rater.Domain.Models;

namespace Rater.Business.Services.Interfaces
{
    public interface IRatingService
    {
        Task<RatingResponseDto> AddRatingsAsync(RatingRequestDto request);
        Task<List<RatingModel>> GetRatingsAsync(int spaceId);
    }
}
