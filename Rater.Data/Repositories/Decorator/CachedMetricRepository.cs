using Microsoft.Extensions.Caching.Memory;
using Rater.API;
using Rater.Data.Repositories.Interfaces;
using Rater.Domain.DataTransferObjects.MetricDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Task<List<MetricResponseDto>> CreateMetrics(List<MetricRequestDto> request)
        {
            return _decorated.CreateMetrics(request);
        }

        public async Task<List<Metric>> GetAllMetrics(int space_id)
        {
            string key = $"metric{space_id}";
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
