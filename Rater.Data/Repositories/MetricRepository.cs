﻿using AutoMapper;
using Rater.API;
using Rater.Data.DataContext;
using Rater.Data.Repositories.Interfaces;
using Rater.Domain.DataTransferObjects.MetricDto;
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
        private readonly IMapper _mapper;
        public MetricRepository(DBBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<Metric>> GetAllMetrics(int space_id)
        {

            var metrics = await _context.Metrics
                .Where(e => e.SpaceId == space_id)
                .Include(e => e.Ratings)
                .ToListAsync();

            return metrics;
        }

        public async Task<List<MetricResponseDto>> CreateMetrics(List<MetricRequestDto> request)
        {
            var metrics = request.Select(e => _mapper.Map<Metric>(e)).ToList();

            await _context.Metrics.AddRangeAsync(metrics);
            await _context.SaveChangesAsync();

            var result = metrics.Select(e => _mapper.Map<MetricResponseDto>(e)).ToList();
            return result;

        }


    }
}
