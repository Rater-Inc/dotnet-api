using Rater.Domain.DataTransferObjects.SpaceDto;
using Rater.Domain.Models;
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
        Task<SpaceModel> GetSpaceByLink(string link);
        Task<bool> SpaceExist(int space_id);


    }
}
