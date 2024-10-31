using Rater.API;
using Rater.Domain.DataTransferObjects.MetricDto;

namespace Rater.Data.Repositories.Interfaces
{
    public interface IMetricRepository
    {
        Task<List<Metric>> GetAllMetrics(int space_id);
        Task<List<MetricResponseDto>> CreateMetrics(List<MetricRequestDto> request);
        Task<List<Metric>> GetMetricsGivenIds(List<int> metricsIds);
    }
}
