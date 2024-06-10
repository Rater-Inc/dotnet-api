using Rater.Business.Services.Interfaces;
using Rater.Data.Repositories.Interfaces;
using Rater.Domain.DataTransferObjects.ParticipantDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Business.Services
{
    public class ParticipantService : IParticipantService
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly ISpaceRepository _spaceRepository;
        public ParticipantService(IParticipantRepository participantRepository, ISpaceRepository spaceRepository)
        {
            _participantRepository = participantRepository;
            _spaceRepository = spaceRepository;
        }

        public async Task<List<ParticipantResponseDto>> CreateParticipants(List<ParticipantRequestDto> request)
        {
            var value = await _participantRepository.CreateParticipants(request);
            return value;
        }

        public async Task<List<ParticipantResponseDto>> GetParticipants(int space_id)
        {
            if(await _spaceRepository.SpaceExist(space_id))
            {
                var value = await _participantRepository.GetParticipants(space_id);
                return value;

            }

            else
            {
                throw new Exception("space does not exist");
            }
        }
    }
}
