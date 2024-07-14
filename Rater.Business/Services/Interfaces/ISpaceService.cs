using Rater.Domain.DataTransferObjects.ResultDto;
using Rater.Domain.DataTransferObjects.SpaceDto;

namespace Rater.Business.Services.Interfaces
{
    public interface ISpaceService
    {
        Task<SpaceResponseDto> AddSpace(GrandSpaceRequestDto request);
        Task<SpaceResponseDto> GetSpace(string link);
        Task<GrandResultResponseDto> GetSpaceResults(string link);
    }
}
