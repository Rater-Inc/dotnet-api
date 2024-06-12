using AutoMapper;
using Rater.API;
using Rater.Data.DataContext;
using Rater.Data.Repositories.Interfaces;
using Rater.Domain.DataTransferObjects.UserDto;
namespace Rater.Data.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly DBBContext _context;
        IMapper _mapper;
        public UserRepository(DBBContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<UserResponseDto> AddUser(UserRequestDto request)
        {
            var user = _mapper.Map<User>(request);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            var returner = _mapper.Map<UserResponseDto>(user);
            return returner;
        }

        public async Task<int> GetCreatorId(DateTime? dateTime)
        {
            var userId = await _context.Users.Where(e => e.CreatedAt == dateTime).FirstOrDefaultAsync();
            return userId.UserId;
        }
    }
}
