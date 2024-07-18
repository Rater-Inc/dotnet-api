using Rater.Data.Repositories.GenericRepositories;
using Rater.Domain.Models;

namespace Rater.Data.Repositories.MetricRepositories
{
    public interface IMetricRepository : IGenericRepository<MetricModel>
    {
        Task<List<MetricModel>> GetAllMetricsAsync(int spaceId);
    }
}
