using Rater.Data.Repositories.GenericRepositories;
using Rater.Domain.DataTransferObjects.UserDto;
using Rater.Domain.Models;

namespace Rater.Data.Repositories.UserRepositories
{
    public interface IUserRepository : IGenericRepository<UserModel>
    {
    }
}
