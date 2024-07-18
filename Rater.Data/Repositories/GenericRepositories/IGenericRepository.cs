using Rater.Domain.Models;
using System.Linq.Expressions;

namespace Rater.Data.Repositories.GenericRepositories;

public interface IGenericRepository<T> where T : BaseModel
{
    DbSet<T> Table { get; }

    Task<T> CreateAsync(T model);
    Task<IEnumerable<T>> CreateRangeAsync(IEnumerable<T> model);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, bool tracking = true);
    Task<T> GetAsync(int id, Expression<Func<T, bool>>? predicate = null, bool tracking = true);
    Task<bool> IsExistAsync(int id);
    T Update(T model);
    bool Delete(T model);
}
