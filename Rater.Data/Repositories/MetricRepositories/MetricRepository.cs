using Rater.Data.DataContext;
using Rater.Data.Repositories.GenericRepositories;
using Rater.Domain.Models;

namespace Rater.Data.Repositories.MetricRepositories
{
    public class MetricRepository : GenericRepository<MetricModel>, IMetricRepository
    {
        public MetricRepository(DBBContext context) : base(context)
        {

        }

        public async Task<List<MetricModel>> GetAllMetricsAsync(int spaceId)
        {
            var metrics = await Table
                .Where(e => e.SpaceId == spaceId)
                .Include(e => e.Ratings)
                .ToListAsync();

            return metrics;
        }
    }
}
