using AutoMapper;
using Rater.Data.DataContext;
using Rater.Data.Repositories.Interfaces;
using Rater.Domain.DataTransferObjects.RatingDto;
using Rater.Domain.Models;
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

        public async Task<RatingResponseDto> AddRatings(List<RatingModel> request)
        {
            if(request.Any())
            {
                foreach (var x in request)
                {
                    var metric = await _context.Metrics.Where(e => e.Id == x.MetricId).FirstOrDefaultAsync();
                    var participant = await _context.Participants.Where(e => e.Id == x.RateeId).FirstOrDefaultAsync();

                    if (metric?.SpaceId != x.SpaceId || participant?.SpaceId != x.SpaceId)
                    {
                        throw new InvalidOperationException("The request payload does not match the provided space ID.");
                    }
                }
                await _context.Ratings.AddRangeAsync(request);
                await _context.SaveChangesAsync();

                var spaceId = request[0].SpaceId;

                return new RatingResponseDto
                {
                    success = true,
                    spaceId = spaceId,
                    ratingCount = request.Count()
                };

            }

            else
            {
                throw new Exception("Request is empty");
            }
        }

        public async Task<List<RatingModel>> GetRatings(int space_id)
        {
            var ratings = await _context.Ratings.Where(e => e.SpaceId == space_id).ToListAsync();
            return ratings;

        }



    }
}
