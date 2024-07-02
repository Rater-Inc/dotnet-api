using Rater.API;
using Rater.Data.Repositories.Interfaces;
using Rater.Data.DataContext;
using AutoMapper;
using Rater.Domain.DataTransferObjects.SpaceDto;
using Rater.Domain.DataTransferObjects.MetricDto;
using RandomString4Net;
using BCrypt.Net;



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

        public async Task<SpaceResponseDto> CreateSpace(SpaceRequestDto request)
        {

            var duplicateNickName = request.Participants.GroupBy(e => e.ParticipantName).Where(g => g.Count() > 1).Select(g => g.Key).ToList();

            if (duplicateNickName.Any())
            {
                throw new ArgumentException("there are same nickname participant in the request");
            }

            request.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var x = _mapper.Map<Space>(request);
            x.Link = RandomString.GetString(Types.ALPHANUMERIC_LOWERCASE);
            await _context.Spaces.AddAsync(x);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<SpaceResponseDto>(x);
            return result;

        }

        public async Task<Space> GetSpaceByLink(string link)
        {
            var space = await _context.Spaces.Where(s => s.Link == link).Include(e=> e.Creator).Include(e => e.Metrics).Include(e => e.Participants).FirstOrDefaultAsync();
            if (space == null) throw new Exception("Space could not found");
            return space;
        } 

        public async Task<bool> SpaceExist(int space_id)
        {
            return await _context.Spaces.AnyAsync(e => e.SpaceId == space_id);
        }

    }
}
