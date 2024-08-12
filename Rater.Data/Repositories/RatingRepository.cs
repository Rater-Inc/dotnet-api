using AutoMapper;
using Rater.API;
using Rater.Data.DataContext;
using Rater.Data.Repositories.Interfaces;
using Rater.Domain.DataTransferObjects.RatingDto;

namespace Rater.Data.Repositories
{
    public class RatingRepository : IRatingRepository
    {

        private readonly DBBContext _context;
        private readonly IMapper _mapper;
        public RatingRepository(DBBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<RatingResponseDto> AddRatings(List<Rating> request)
        {

            await _context.Ratings.AddRangeAsync(request);
            await _context.SaveChangesAsync();

            var spaceId = request[0].SpaceId;

            return new RatingResponseDto
            {
                success = true,
                spaceId = spaceId,
                ratingCount = request.Count()
            };
        }

        public async Task<List<Rating>> GetRatings(int space_id)
        {
            var ratings = await _context.Ratings.Where(e => e.SpaceId == space_id).ToListAsync();
            return ratings;

        }



    }
}
