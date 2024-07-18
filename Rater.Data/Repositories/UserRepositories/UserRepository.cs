using AutoMapper;
using Rater.Data.DataContext;
using Rater.Data.Repositories.GenericRepositories;
using Rater.Domain.Models;
namespace Rater.Data.Repositories.UserRepositories
{
    public class UserRepository : GenericRepository<UserModel>, IUserRepository
    {
        public UserRepository(DBBContext context, IMapper mapper) : base(context)
        {

        }
    }
}
