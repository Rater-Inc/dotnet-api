using AutoMapper;
using Rater.Business.Services.Interfaces;
using Rater.Data.Repositories.RatingRepositories;
using Rater.Domain.DataTransferObjects.RatingDto;
using Rater.Domain.DataTransferObjects.UserDto;
using Rater.Domain.Models;

namespace Rater.Business.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public RatingService(
            IRatingRepository repo,
            IUserService userService,
            IMapper mapper,
            IAuthService authService)
        {
            _ratingRepository = repo;
            _userService = userService;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<RatingResponseDto> AddRatingsAsync(RatingRequestDto request)
        {
            await _authService.ValidateAuthorizationAsync(request.SpaceId);

            if (request.RatingDetails == null || request.RatingDetails.Count == 0)
            {
                throw new ArgumentException("Rating values are empty");
            }

            UserRequestDto userRequest = new()
            {
                NickName = request.RaterNickName
            };

            var user = await _userService.CreateUserAsync(userRequest);

            var ratings = request.RatingDetails.Select(e =>
            {
                var rating = _mapper.Map<RatingModel>(e);
                rating.RaterId = user.UserId;
                rating.SpaceId = request.SpaceId;
                return rating;

            }).ToList();

            var invalidScores = ratings.Where(e => e.Score <= 0 || e.Score > 5).ToList();

            if (invalidScores.Count != 0)
            {
                throw new ArgumentException($"Found {invalidScores.Count} scores not between 1 and 5");
            }

            var returner = await _ratingRepository.AddRatingsAsync(ratings);
            return returner;
        }

        public async Task<List<RatingModel>> GetRatingsAsync(int spaceId)
        {
            var ratings = await _ratingRepository.GetAllRatingsAsync(spaceId);
            return ratings;
        }
    }
}
