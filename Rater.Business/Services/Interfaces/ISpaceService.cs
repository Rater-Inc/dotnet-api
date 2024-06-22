using Rater.API;
using Rater.Domain.DataTransferObjects.ResultDto;
using Rater.Domain.DataTransferObjects.SpaceDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Business.Services.Interfaces
{
    public interface ISpaceService
    {
        Task<SpaceResponseDto> AddSpace(GrandSpaceRequestDto request);
        Task<SpaceResponseDto> GetSpace(string link);
        Task<GrandResultResponseDto> GetSpaceResults(string link);
    }
}
