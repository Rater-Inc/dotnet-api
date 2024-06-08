using Rater.API;

namespace Rater.Data.Repositories.Interfaces
{
    public interface IMetricRepository
    {
        Task<List<Metric>> GetAllMetrics();
    }
}
