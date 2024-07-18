using AutoMapper;
using Rater.Data.DataContext;
using Rater.Data.Repositories.GenericRepositories;
using Rater.Domain.DataTransferObjects.UserDto;
using Rater.Domain.Models;
namespace Rater.Data.Repositories.UserRepositories
{
    public class UserRepository : GenericRepository<UserModel>, IUserRepository
    {
        private readonly DBBContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DBBContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserResponseDto> AddUserAsync(UserRequestDto request)
        {
            //todo: mapping islemleri service altinda yapilabilir.
            var user = _mapper.Map<UserModel>(request);

            await Table.AddAsync(user);
            await _context.SaveChangesAsync();

            var returner = _mapper.Map<UserResponseDto>(user);

            return returner;
        }
    }
}
