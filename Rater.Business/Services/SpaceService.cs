using Rater.Business.Services.Interfaces;
using Rater.Data.Repositories.Interfaces;
using Rater.Domain.DataTransferObjects.SpaceDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Business.Services
{
    public class SpaceService : ISpaceService
    {

        private readonly ISpaceRepository _repo;
        public SpaceService(ISpaceRepository repo)
        {
            _repo = repo;
        }



        public async Task<List<SpaceResponseDto>> GetAllSpaces()
        {
            var value = await _repo.GetAllSpaces();
            return value;
        }
    }
}
