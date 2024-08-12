using Rater.API;

namespace Rater.Business.Services.Interfaces
{
    public interface IMetricService
    {
        Task<List<Metric>> GetMetrics(int space_id);
        Task<List<Metric>> GetMetricsGivenIds(List<int> metricsIds);
    }
}
