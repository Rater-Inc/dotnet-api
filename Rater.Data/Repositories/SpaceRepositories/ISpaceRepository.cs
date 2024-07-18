using Rater.Data.Repositories.GenericRepositories;
using Rater.Domain.DataTransferObjects.SpaceDto;
using Rater.Domain.Models;

namespace Rater.Data.Repositories.SpaceRepositories
{
    public interface ISpaceRepository : IGenericRepository<SpaceModel>
    {
        Task<SpaceResponseDto> CreateSpaceAsync(SpaceRequestDto request);
        Task<SpaceModel> GetSpaceByLinkAsync(string link);
    }
}
