using AutoMapper;
using Rater.API;
using Rater.Data.DataContext;
using Rater.Data.Repositories.Interfaces;
using Rater.Domain.DataTransferObjects.RatingDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Data.Repositories
{
    public class RatingRepository : IRatingRepository
    {

        private readonly DBBContext _context;
        private readonly IMapper _mapper;
        public RatingRepository(DBBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<RatingForMetricResponseDto>> AddRatings(List<Rating> request)
        {

            foreach (var x in request)
            {
                var metric = await _context.Metrics.Where(e => e.MetricId == x.MetricId).FirstOrDefaultAsync();
                var participant = await _context.Participants.Where(e => e.ParticipantId == x.RateeId).FirstOrDefaultAsync();

                if(metric?.SpaceId != x.SpaceId || participant?.SpaceId != x.SpaceId)
                {
                    throw new Exception("The request payload does not match the provided space ID.");
                }
            }

            await _context.Ratings.AddRangeAsync(request);
            await _context.SaveChangesAsync();

            //var ratingResponse = request.Select(e => _mapper.Map<RatingForMetricResponseDto>(e)).ToList(); 

            var firstRating = request.FirstOrDefault();
            var spaceID = firstRating?.SpaceId;

            var ratingResponse = await _context.Ratings
                                        .Where(e => e.SpaceId == spaceID)
                                        .Include(e => e.Ratee)
                                        .Include(e => e.Rater)
                                        .Select(e => _mapper.Map<RatingForMetricResponseDto>(e))
                                        .ToListAsync();

            return ratingResponse;
        }

        public async Task<List<Rating>> GetRatings(int space_id)
        {
            var ratings = await _context.Ratings.Where(e => e.SpaceId == space_id).ToListAsync();
            return ratings;

        }



    }
}
