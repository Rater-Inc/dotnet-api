using Rater.API;
using Rater.Domain.DataTransferObjects.RatingDto;
using Rater.Domain.DataTransferObjects.ResultDto;
using Rater.Domain.DataTransferObjects.SpaceDto;

namespace Rater.Business.Services.Interfaces
{
    public interface ISpaceService
    {
        Task<SpaceResponseDto> AddSpace(GrandSpaceRequestDto request);
        Task<SpaceResponseDto> GetSpace(string link);
        Task<GrandResultResponseDto> GetSpaceResults(string link);
        Task<RatingResponseDto> AddRatings(RatingRequestDto request);
        Task<Space?> GetSpaceByLink(string link);
        Task<bool> SpaceExist(int space_id);
    }
}
