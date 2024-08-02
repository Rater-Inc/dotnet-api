using Microsoft.Extensions.Caching.Memory;
using Rater.API;
using Rater.Data.Repositories.Interfaces;
using Rater.Domain.DataTransferObjects.ParticipantDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Data.Repositories.Decorator
{
    public class CachedParticipantRepository : IParticipantRepository
    {
        private readonly ParticipantRepository _decorated;
        private readonly IMemoryCache _memoryCache;
        public CachedParticipantRepository(ParticipantRepository decorated, IMemoryCache memoryCache)
        {
            _decorated = decorated;
            _memoryCache = memoryCache;
        }

        public async Task<List<ParticipantResponseDto>> CreateParticipants(List<ParticipantRequestDto> request)
        {
            return await _decorated.CreateParticipants(request);
        }

        public async Task<List<Participant>> GetParticipants(int space_id)
        {
            string key = $"participant{space_id}";
            var cachedParticipants = await _memoryCache.GetOrCreateAsync(
                key,
                async entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
                    return await _decorated.GetParticipants(space_id);
                });
            return cachedParticipants == null ? throw new Exception("Failed to retrieve Space") : cachedParticipants;

        }
    }
}
