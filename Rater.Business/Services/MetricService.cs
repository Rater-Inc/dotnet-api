using Rater.API;
using Rater.Business.Services.Interfaces;
using Rater.Data.Repositories.Interfaces;

namespace Rater.Business.Services
{
    public class MetricService : IMetricService
    {

        private readonly IMetricRepository _metricRepository;
        private readonly ISpaceRepository _spaceRepository;
        public MetricService(IMetricRepository metricRepository , ISpaceRepository spaceRepository)
        {
            _metricRepository = metricRepository;
            _spaceRepository = spaceRepository;
        }


        public async Task<List<Metric>> GetMetrics(int space_id)
        {
            if (await _spaceRepository.SpaceExist(space_id))
            {
                var value = await _metricRepository.GetAllMetrics(space_id);
                return value;
            }

            else
            {
                throw new Exception("space does not exist");
            }
            
        }
    }
}
