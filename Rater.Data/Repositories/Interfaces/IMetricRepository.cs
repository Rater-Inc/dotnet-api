using Rater.API;
using Rater.Domain.DataTransferObjects.MetricDto;

namespace Rater.Data.Repositories.Interfaces
{
    public interface IMetricRepository
    {
        Task<List<MetricResponseDto>> GetAllMetrics(int space_id);
        Task<List<MetricResponseDto>> CreateMetrics(List<MetricRequestDto> request);
    }
}
