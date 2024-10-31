using Rater.API;
using Rater.Business.Services.Interfaces;
using Rater.Data.Repositories.Interfaces;

namespace Rater.Business.Services
{
    public class ParticipantService : IParticipantService
    {
        private readonly IParticipantRepository _participantRepository;
        public ParticipantService(IParticipantRepository participantRepository)
        {
            _participantRepository = participantRepository;
        }

        public async Task<List<Participant>> GetParticipants(int space_id)
        {
            var value = await _participantRepository.GetAllParticipants(space_id);
            if (!value.Any())
            {
                throw new InvalidOperationException("Couldn't retrieve participants.");
            }
            return value;
        }

        public async Task<List<Participant>> GetParticipantsGivenIds(List<int> participantIds)
        {
            var value = await _participantRepository.GetParticipantsGivenIds(participantIds);
            if (!value.Any())
            {
                throw new InvalidOperationException("Couldn't retrieve participants.");
            }
            return value;
        }
    }
}
