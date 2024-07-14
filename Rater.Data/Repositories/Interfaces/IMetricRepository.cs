using Rater.Domain.DataTransferObjects.MetricDto;
using Rater.Domain.Models;

namespace Rater.Data.Repositories.Interfaces
{
    public interface IMetricRepository
    {
        Task<List<MetricModel>> GetAllMetrics(int space_id);
        Task<List<MetricResponseDto>> CreateMetrics(List<MetricRequestDto> request);
    }
}
