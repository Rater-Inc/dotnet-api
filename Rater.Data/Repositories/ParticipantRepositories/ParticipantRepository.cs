using Rater.Data.DataContext;
using Rater.Data.Repositories.GenericRepositories;
using Rater.Domain.Models;

namespace Rater.Data.Repositories.ParticipantRepositories
{
    public class ParticipantRepository : GenericRepository<ParticipantModel>, IParticipantRepository
    {
        public ParticipantRepository(DBBContext context) : base(context)
        {
        }

        public async Task<List<ParticipantModel>> GetAllParticipantsAsync(int spaceId)
        {
            var participants = await Table.Where(e => e.SpaceId == spaceId).ToListAsync();
            return participants;
        }
    }
}
