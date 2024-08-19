using Microsoft.Extensions.Caching.Memory;
using Rater.API;
using Rater.Data.Repositories.Interfaces;
using Rater.Domain.DataTransferObjects.SpaceDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Data.Repositories.Decorator
{
    public class CachedSpaceRepository : ISpaceRepository
    {
        private readonly SpaceRepository _decorated;
        private readonly IMemoryCache _memoryCache;
        public CachedSpaceRepository(SpaceRepository decorated, IMemoryCache memoryCache)
        {
            _decorated = decorated;
            _memoryCache = memoryCache;
        }
        public async Task<SpaceResponseDto> CreateSpace(Space request)
        {
            return await _decorated.CreateSpace(request);
        }

        public async Task<Space?> GetSpaceByLink(string link)
        {
            string key = $"space{link}";
            var cachedSpace = await _memoryCache.GetOrCreateAsync(
                key,
                async entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
                    return await _decorated.GetSpaceByLink(link);
                });
            return cachedSpace;
        }

        public async Task<bool> SpaceExist(int space_id)
        {
            string key = $"space:exists:{space_id}";
            var cachedSpaceExists = await _memoryCache.GetOrCreateAsync(
                key,
                async entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromHours(1));
                    return await _decorated.SpaceExist(space_id);
                });
            return cachedSpaceExists;
        }
    }
}
