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
            IMapper mapper,
            IAuthService authService)
        {
            _repo = repo;
            _userService = userService;
            _mapper = mapper;
        }


        public async Task<RatingResponseDto> AddRatings(RatingRequestDto request)
        {

            if (request.RatingDetails == null || !request.RatingDetails.Any())
            {
                throw new ArgumentException("Rating values are empty");
            }

            try
            {

                UserRequestDto userRequest = new UserRequestDto();
                userRequest.NickName = request.RaterNickName;
                var user = await _userService.CreateUser(userRequest);

                var ratings = request.RatingDetails.Select(e =>
                {
                    var rating = _mapper.Map<Rating>(e);
                    rating.RaterId = user.UserId;
                    rating.SpaceId = request.SpaceId;
                    return rating;

                }).ToList();

                var invalidScores = ratings.Where(e => e.Score <= 0 || e.Score > 5).ToList();

                if (invalidScores.Any())
                {
                    throw new ArgumentException($"Found {invalidScores.Count} scores not between 1 and 5");
                }

                var returner = await _repo.AddRatings(ratings);
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
