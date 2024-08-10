using Microsoft.Extensions.Caching.Memory;
using Rater.API;
using Rater.Data.Repositories.Interfaces;
using Rater.Domain.DataTransferObjects.MetricDto;

namespace Rater.Data.Repositories.Decorator
{
    public class CachedMetricRepository : IMetricRepository
    {
        private readonly MetricRepository _decorated;
        private readonly IMemoryCache _memoryCache;
        public CachedMetricRepository(MetricRepository decorated, IMemoryCache memoryCache)
        {
            _decorated = decorated;
            _memoryCache = memoryCache;
        }

        public async Task<List<MetricResponseDto>> CreateMetrics(List<MetricRequestDto> request)
        {
            return await _decorated.CreateMetrics(request);
        }

        public async Task<List<Metric>> GetMetricsGivenIds(List<int> metricsIds)
        {
            return await _decorated.GetMetricsGivenIds(metricsIds);
        }

        public async Task<List<Metric>?> GetAllMetrics(int space_id)
        {
            string key = $"metric{space_id}";
            if (_memoryCache.TryGetValue(key, out List<Metric>? isCached))
            {
                if (isCached == null)
                {
                    _memoryCache.Remove(key);
                }
            }
            var cachedMetrics = await _memoryCache.GetOrCreateAsync(
                key,
                async entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
                    return await _decorated.GetAllMetrics(space_id);
                });
            return cachedMetrics == null ? throw new Exception("Failed to retrieve Space") : cachedMetrics;
        }
    }
}
