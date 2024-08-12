using AutoMapper;
using Rater.API;
using Rater.Business.Services.Interfaces;
using Rater.Data.Repositories.Interfaces;
using Rater.Domain.DataTransferObjects.RatingDto;
using Rater.Domain.DataTransferObjects.UserDto;

namespace Rater.Business.Services
{
    public class RatingService : IRatingService
    {

        private readonly IRatingRepository _repo;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public RatingService(
            IRatingRepository repo,
            IUserService userService,
            IMapper mapper)
        {
            _repo = repo;
            _userService = userService;
            _mapper = mapper;
        }


        public async Task<RatingResponseDto> AddRatings(List<Rating> request)
        {
            try
            {
                var returner = await _repo.AddRatings(request);
                return returner;
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<Rating>> GetRatings(int space_id)
        {
            var ratings = await _repo.GetRatings(space_id);
            return ratings;
        }

    }
}
