using Rater.Data.Repositories.GenericRepositories;
using Rater.Domain.Models;

namespace Rater.Data.Repositories.ParticipantRepositories
{
    public interface IParticipantRepository : IGenericRepository<ParticipantModel>
    {
        Task<List<ParticipantModel>> GetAllParticipantsAsync(int spaceId);
    }
}
