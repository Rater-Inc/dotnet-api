using Rater.Domain.Models;

namespace Rater.Business.Services.Interfaces
{
    public interface IParticipantService
    {
        Task<List<ParticipantModel>> GetParticipantsAsync(int spaceId);
    }
}
