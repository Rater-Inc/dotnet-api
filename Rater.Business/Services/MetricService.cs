using Rater.API;
using Rater.Business.Services.Interfaces;
using Rater.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Business.Services
{
    public class MetricService : IMetricService
    {

        private readonly IMetricRepository _repo;
        public MetricService(IMetricRepository repo)
        {
            _repo = repo;
        }


        public async Task<List<Metric>> GetMetrics()
        {
            var value = await _repo.GetAllMetrics();
            return value;
        }
    }
}
