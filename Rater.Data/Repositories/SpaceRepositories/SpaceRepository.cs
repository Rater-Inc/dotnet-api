using Rater.Data.DataContext;
using AutoMapper;
using Rater.Domain.DataTransferObjects.SpaceDto;
using RandomString4Net;
using Rater.Domain.Models;
using Rater.Data.Repositories.GenericRepositories;



namespace Rater.Data.Repositories.SpaceRepositories
{
    public class SpaceRepository : GenericRepository<SpaceModel>, ISpaceRepository
    {

        private readonly DBBContext _context;
        private readonly IMapper _mapper;

        public SpaceRepository(DBBContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SpaceResponseDto> CreateSpaceAsync(SpaceRequestDto request)
        {
            var duplicateNickName = request.Participants.GroupBy(e => e.ParticipantName)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicateNickName.Count != 0)
            {
                throw new ArgumentException("there are same nickname participant in the request");
            }

            request.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var spaceModel = _mapper.Map<SpaceModel>(request);
            spaceModel.Link = RandomString.GetString(Types.ALPHANUMERIC_LOWERCASE);
            
            await Table.AddAsync(spaceModel);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<SpaceResponseDto>(spaceModel);

            return result;

        }

        public async Task<SpaceModel> GetSpaceByLinkAsync(string link)
        {
            var space = await Table
                .Include(e => e.Creator)
                .Include(e => e.Metrics)
                .Include(e => e.Participants)
                .FirstOrDefaultAsync(s => s.Link == link);
            //todo: bunun kontrolu de service altinda olabilir.
            if (space is null) throw new Exception("Space could not found");

            return space;
        }
    }
}
