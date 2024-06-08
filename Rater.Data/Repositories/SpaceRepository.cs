using Rater.API;
using Rater.Data.Repositories.Interfaces;
using Rater.Data.DataContext;
using AutoMapper;
using Rater.Domain.DataTransferObjects.SpaceDto;
using Rater.Domain.DataTransferObjects.MetricDto;
using RandomString4Net;



namespace Rater.Data.Repositories
{
    public class SpaceRepository : ISpaceRepository
    {

        private readonly DBBContext _context;
        private readonly IMapper _mapper;

        public SpaceRepository(DBBContext context , IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
                .Select(e => _mapper.Map<SpaceResponseDto>(e))
                .ToListAsync();

            return space;

        }

        public async Task<SpaceResponseDto> CreateSpace(SpaceRequestDto request)
        {
            var x = _mapper.Map<Space>(request);
            x.Link = RandomString.GetString(Types.ALPHANUMERIC_LOWERCASE);
            await _context.Spaces.AddAsync(x);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<SpaceResponseDto>(x);
            return result;

        }

    }
}
