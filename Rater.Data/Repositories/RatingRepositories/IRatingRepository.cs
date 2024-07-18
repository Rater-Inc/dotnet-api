using Rater.Data.Repositories.GenericRepositories;
using Rater.Domain.DataTransferObjects.RatingDto;
using Rater.Domain.Models;

namespace Rater.Data.Repositories.RatingRepositories
{
    public interface IRatingRepository : IGenericRepository<RatingModel>
    {
        Task<RatingResponseDto> AddRatingsAsync(List<RatingModel> request);
        Task<List<RatingModel>> GetAllRatingsAsync(int spaceId);
    }
}
