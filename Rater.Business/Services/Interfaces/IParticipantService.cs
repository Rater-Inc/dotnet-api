using Rater.API;

namespace Rater.Business.Services.Interfaces
{
    public interface IParticipantService
    {
        Task<List<Participant>> GetParticipants(int space_id);
        Task<List<Participant>> GetParticipantsGivenIds(List<int> participantIds);
    }
}
