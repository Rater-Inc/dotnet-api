using Microsoft.EntityFrameworkCore.ChangeTracking;
using Rater.Data.DataContext;
using Rater.Domain.Models;
using System.Linq.Expressions;

namespace Rater.Data.Repositories.GenericRepositories;

public class GenericRepository<T>(DBBContext context) : IGenericRepository<T> where T : BaseModel
{
    readonly private DBBContext _context = context;

    public DbSet<T> Table => _context.Set<T>();

    public async Task<T> CreateAsync(T model)
    {
        await Table.AddAsync(model);
        return model;
    }

    public async Task<IEnumerable<T>> CreateRangeAsync(IEnumerable<T> models)
    {
        await Table.AddRangeAsync(models);
        return models;
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, bool tracking = true)
    {
        var query = Table.AsQueryable();

        if (tracking is false) query = query.AsNoTracking();
        if(predicate is not null) query = query.Where(predicate);

        return await query.ToListAsync();
    }

    public async Task<T> GetAsync(int id, Expression<Func<T, bool>>? predicate = null, bool tracking = true)
    {
        var query = Table.AsQueryable();

        if (tracking is false) query = query.AsNoTracking();
        if (predicate is not null) query = query.Where(predicate);

        return await query.FirstAsync(f => f.Id == id);
    }

    public async Task<bool> IsExistAsync(int id)
    {
        return await Table.AsNoTracking().AnyAsync(w => w.Id == id);
    }

    public T Update(T model)
    {
        Table.Update(model);
        return model;
    }

    public bool Delete(T model)
    {
        EntityEntry entityEntry = Table.Remove(model);
        return entityEntry.State == EntityState.Deleted;
    }
}
