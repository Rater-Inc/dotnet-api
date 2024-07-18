using Rater.Business.Services.Interfaces;
using Rater.Data.Repositories.ParticipantRepositories;
using Rater.Data.Repositories.SpaceRepositories;
using Rater.Domain.Models;

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

        public async Task<List<ParticipantModel>> GetParticipants(int spaceId)
        {
            if (await _spaceRepository.IsExistAsync(spaceId) is false) { throw new Exception("space does not exist"); }

            var value = await _participantRepository.GetAllParticipantsAsync(spaceId);
            return value;
        }
    }
}
