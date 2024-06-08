using Rater.API;
using Rater.Data.DataContext;
using Rater.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Data.Repositories
{
    public class MetricRepository : IMetricRepository
    {

        private readonly DBBContext _context;
        public MetricRepository(DBBContext context)
        {
            _context = context;
        }


        public async Task<List<Metric>> GetAllMetrics()
        {
            var metrics = await _context.Metrics
                .Include(e => e.Ratings)
                .ToListAsync();
            return metrics;
        }



    }
}
