using Rater.API;
using Rater.Business.Services.Interfaces;
using Rater.Data.Repositories.Interfaces;

namespace Rater.Business.Services
{
    public class MetricService : IMetricService
    {

        private readonly IMetricRepository _metricRepository;
        public MetricService(IMetricRepository metricRepository)
        {
            _metricRepository = metricRepository;
        }


        public async Task<List<Metric>> GetMetrics(int space_id)
        {
            var value = await _metricRepository.GetAllMetrics(space_id);
            if (!value.Any())
            {
                throw new InvalidOperationException("Couldn't retrieve metrics.");
            }
            return value;

        }

        public async Task<List<Metric>> GetMetricsGivenIds(List<int> metricsIds)
        {
            var value = await _metricRepository.GetMetricsGivenIds(metricsIds);
            if (!value.Any())
            {
                throw new InvalidOperationException("Couldn't retrieve metrics.");
            }
            return value;
        }
    }
}
