using Rater.API;
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
        Task<List<SpaceResponseDto>> GetAllSpaces();
    }
}
