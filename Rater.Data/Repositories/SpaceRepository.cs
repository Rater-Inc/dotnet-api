using Rater.API;
using Rater.Data.Repositories.Interfaces;
using Rater.Data.DataContext;
using AutoMapper;
using Rater.Domain.DataTransferObjects.SpaceDto;
using Rater.Domain.DataTransferObjects.MetricDto;



namespace Rater.Data.Repositories
{
    public class SpaceRepository : ISpaceRepository
    {

        private readonly DBBContext _context;
        private readonly IMapper _mappper;

        public SpaceRepository(DBBContext context , IMapper mapper)
        {
            _context = context;
            _mappper = mapper;
        }


        public async Task<List<SpaceResponseDto>> GetAllSpaces()
        {
            var space = await _context.Spaces
                .Include(e => e.Metrics)
                .ThenInclude(e => e.Ratings)
                .ThenInclude(e => e.Ratee)
                .Include(e => e.Metrics)
                .ThenInclude(e => e.Ratings)
                .ThenInclude(e => e.Rater)
                .Select(e => _mappper.Map<SpaceResponseDto>(e))
                .ToListAsync();

            return space;

        }
    }
}
