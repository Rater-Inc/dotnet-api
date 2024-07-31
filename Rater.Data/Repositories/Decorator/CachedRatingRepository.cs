using Microsoft.Extensions.Caching.Memory;
using Rater.API;
using Rater.Data.Repositories.Interfaces;
using Rater.Domain.DataTransferObjects.RatingDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Data.Repositories.Decorator
{
    public class CachedRatingRepository : IRatingRepository
    {
        private readonly RatingRepository _decorated;
        private readonly IMemoryCache _memoryCache;
        public CachedRatingRepository(RatingRepository decorated , IMemoryCache memoryCache)
        {
            _decorated = decorated;
            _memoryCache = memoryCache;
        }

        public async Task<RatingResponseDto> AddRatings(List<Rating> request)
        {
            return await _decorated.AddRatings(request);

        }

        public async Task<List<Rating>> GetRatings(int space_id)
        {
            string key = $"rating{space_id}";
            var cachedRatings = await _memoryCache.GetOrCreateAsync(
                key,
                async entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(10));
                    return await _decorated.GetRatings(space_id);
                });
            return cachedRatings == null ? throw new Exception("Failed to retrieve Space") : cachedRatings;
        }
    }
}
