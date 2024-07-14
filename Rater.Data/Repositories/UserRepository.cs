using AutoMapper;
using Rater.Data.DataContext;
using Rater.Data.Repositories.Interfaces;
using Rater.Domain.DataTransferObjects.UserDto;
using Rater.Domain.Models;
namespace Rater.Data.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly DBBContext _context;
        IMapper _mapper;
        public UserRepository(DBBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<UserResponseDto> AddUser(UserRequestDto request)
        {
            var user = _mapper.Map<UserModel>(request);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            var returner = _mapper.Map<UserResponseDto>(user);
            return returner;
        }
    }
}
