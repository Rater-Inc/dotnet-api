using Rater.Domain.Models;

namespace Rater.Business.Services.Interfaces
{
    public interface IMetricService
    {
        Task<List<MetricModel>> GetMetricsAsync(int spaceId);
    }
}
