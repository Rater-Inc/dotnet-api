using Rater.Domain.DataTransferObjects.ResultDto;
using Rater.Domain.DataTransferObjects.SpaceDto;

namespace Rater.Business.Services.Interfaces
{
    public interface ISpaceService
    {
        Task<SpaceResponseDto> AddSpaceAsync(GrandSpaceRequestDto request);
        Task<SpaceResponseDto> GetSpaceAsync(string link);
        Task<GrandResultResponseDto> GetSpaceResultsAsync(string link);
    }
}
