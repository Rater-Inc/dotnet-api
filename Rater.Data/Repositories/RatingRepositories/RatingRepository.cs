using Rater.Data.DataContext;
using Rater.Data.Repositories.GenericRepositories;
using Rater.Domain.DataTransferObjects.RatingDto;
using Rater.Domain.Models;

namespace Rater.Data.Repositories.RatingRepositories
{
    public class RatingRepository : GenericRepository<RatingModel>, IRatingRepository
    {
        private readonly DBBContext _context;
        public RatingRepository(DBBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<RatingResponseDto> AddRatingsAsync(List<RatingModel> request)
        {
            //todo: bu kontrolleri repositoryden cikartabiliriz.
            if (request.Count == 0) throw new Exception("Request is empty");

            foreach (var x in request)
            {
                var metric = await _context.Metrics.FirstOrDefaultAsync(f => f.Id == x.MetricId);
                var participant = await _context.Participants.FirstOrDefaultAsync(f => f.Id == x.RateeId);

                if (metric?.SpaceId != x.SpaceId || participant?.SpaceId != x.SpaceId)
                {
                    throw new InvalidOperationException("The request payload does not match the provided space ID.");
                }
            }

            await Table.AddRangeAsync(request);
            await _context.SaveChangesAsync();

            var spaceId = request[0].SpaceId;

            return new RatingResponseDto
            {
                success = true,
                spaceId = spaceId,
                ratingCount = request.Count
            };
        }

        public async Task<List<RatingModel>> GetAllRatingsAsync(int spaceId)
        {
            var ratings = await Table.Where(e => e.SpaceId == spaceId).ToListAsync();
            return ratings;
        }
    }
}
