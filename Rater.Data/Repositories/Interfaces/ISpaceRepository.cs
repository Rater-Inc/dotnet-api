using Rater.API;
using Rater.Domain.DataTransferObjects.SpaceDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Data.Repositories.Interfaces
{
    public interface ISpaceRepository
    {

        Task<SpaceResponseDto> CreateSpace(SpaceRequestDto request);
        Task<Space> GetSpaceByLink(string link);
        Task<bool> SpaceExist(int space_id);


    }
}
