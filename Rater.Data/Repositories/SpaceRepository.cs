using Rater.API;
using Rater.Data.Repositories.Interfaces;
using Rater.Data.DataContext;
using AutoMapper;
using Rater.Domain.DataTransferObjects.SpaceDto;



namespace Rater.Data.Repositories
{
    public class SpaceRepository : ISpaceRepository
    {

        private readonly DBBContext _context;
        private readonly IMapper _mapper;

        public SpaceRepository(DBBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SpaceResponseDto> CreateSpace(Space request)
        {
            await _context.Spaces.AddAsync(request);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<SpaceResponseDto>(request);
            return result;
        }

        public async Task<Space> GetSpaceByLink(string link)
        {
            var space = await _context.Spaces.Where(s => s.Link == link).Include(e => e.Creator).Include(e => e.Metrics).Include(e => e.Participants).FirstOrDefaultAsync();
            if (space == null) throw new Exception("Space could not found");
            return space;
        }

        public async Task<bool> SpaceExist(int space_id)
        {
            return await _context.Spaces.AnyAsync(e => e.SpaceId == space_id);
        }

    }
}
