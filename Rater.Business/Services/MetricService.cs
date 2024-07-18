using Rater.Business.Services.Interfaces;
using Rater.Data.Repositories.MetricRepositories;
using Rater.Data.Repositories.SpaceRepositories;
using Rater.Domain.Models;

namespace Rater.Business.Services
{
    public class MetricService : IMetricService
    {
        private readonly IMetricRepository _metricRepository;
        private readonly ISpaceRepository _spaceRepository;
        public MetricService(IMetricRepository metricRepository, ISpaceRepository spaceRepository)
        {
            _metricRepository = metricRepository;
            _spaceRepository = spaceRepository;
        }

        public async Task<List<MetricModel>> GetMetricsAsync(int spaceId)
        {
            if (await _spaceRepository.IsExistAsync(spaceId) is false) { throw new Exception("space does not exist"); }

            var value = await _metricRepository.GetAllMetricsAsync(spaceId);
            return value;
        }
    }
}
