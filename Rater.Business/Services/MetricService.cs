using Rater.API;
using Rater.Business.Services.Interfaces;
using Rater.Data.Repositories.Interfaces;
using Rater.Domain.DataTransferObjects.MetricDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        public async Task<List<MetricResponseDto>> GetMetrics(int space_id)
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

        public async Task<List<MetricResponseDto>> CreateMetrics(List<MetricRequestDto> request)
        {
            var value = await _metricRepository.CreateMetrics(request);
            return value;
        }
    }
}
