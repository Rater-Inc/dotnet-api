using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Rater.Business.Services.Interfaces;
using Rater.Domain.DataTransferObjects.MetricDto;
using Rater.Domain.DataTransferObjects.ParticipantDto;
using Rater.Domain.DataTransferObjects.ResultDto;
using Rater.Domain.DataTransferObjects.SpaceDto;
using Rater.Domain.Models;
using Rater.Domain.DataTransferObjects.UserDto;
using Rater.Data.Repositories.SpaceRepositories;

namespace Rater.Business.Services
{
    public class SpaceService : ISpaceService
    {
        private readonly ISpaceRepository _spaceRepository;
        private readonly IUserService _userService;
        private readonly IRatingService _ratingService;
        private readonly IParticipantService _participantService;
        private readonly IMapper _mapper;
        private readonly IMetricService _metricService;
        private readonly IAuthService _authService;

        public SpaceService(
            ISpaceRepository spaceRepo,
            IUserService userService,
            IMetricService metricService,
            IRatingService ratingService,
            IParticipantService participantService,
            IMapper mapper,
            IAuthService authService)
        {
            _spaceRepository = spaceRepo;
            _userService = userService;
            _metricService = metricService;
            _ratingService = ratingService;
            _participantService = participantService;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<SpaceResponseDto> AddSpaceAsync(GrandSpaceRequestDto request)
        {
            UserRequestDto userRequest = new()
            {
                NickName = request.creatorNickname
            };
            var justCreatedUser = await _userService.CreateUserAsync(userRequest);

            var space = _mapper.Map<SpaceModel>(request);
            space.CreatorId = justCreatedUser.UserId;

            foreach (var metrics in space.Metrics)
            {
                metrics.SpaceId = space.Id;
            }

            foreach (var participants in space.Participants)
            {
                participants.SpaceId = space.Id;
            }

            var finalRequest = _mapper.Map<SpaceRequestDto>(space);
            var result = await _spaceRepository.CreateSpaceAsync(finalRequest);

            return result;
        }

        public async Task<SpaceResponseDto> GetSpaceAsync(string link)
        {
            if (await _spaceRepository.IsExistAsync(link) is false) throw new Exception("Space could not found");

            var space = await _spaceRepository.GetSpaceByLinkAsync(link);
            //todo: bunun için custom authorize attribute yazılabilir.
            await _authService.ValidateAuthorizationAsync(space.Id);

            var returner = _mapper.Map<SpaceResponseDto>(space);
            return returner;
        }

        public async Task<GrandResultResponseDto> GetSpaceResultsAsync(string link)
        {
            if (await _spaceRepository.IsExistAsync(link) is false) throw new Exception("Space could not found");

            var space = await _spaceRepository.GetSpaceByLinkAsync(link);
            //todo: bunun için custom authorize attribute yazılabilir.
            await _authService.ValidateAuthorizationAsync(space.Id);

            GrandResultResponseDto response = new()
            {
                SpaceId = space.Id,
                Name = space.Name
            };

            var metrics = await _metricService.GetMetricsAsync(space.Id);
            var metricResponse = _mapper.Map<List<MetricLeaderDto>>(metrics);
            response.MetricLeaders = metricResponse;

            var participants = await _participantService.GetParticipantsAsync(space.Id);
            var participantResponse = _mapper.Map<List<PariticipantResultDto>>(participants);
            response.ParticipantResults = participantResponse;

            var ratingsInSpace = await _ratingService.GetRatingsAsync(space.Id);

            foreach (var metric in response.MetricLeaders)
            {
                var metricRatings = ratingsInSpace.Where(e => e.MetricId == metric.Id).ToList();

                if (metricRatings.Count != 0)
                {
                    var averageScore = metricRatings
                        .GroupBy(e => e.Ratee)
                        .Select(g => new
                        {
                            Ratee = g.Key,
                            AverageScore = g.Average(r => r.Score)
                        })
                        .OrderByDescending(x => x.AverageScore)
                        .FirstOrDefault();

                    metric.LeaderParticipant = _mapper.Map<ParticipantResponseDto>(averageScore?.Ratee);
                    metric.Score = averageScore?.AverageScore ?? 0;
                }
                else
                {
                    metric.LeaderParticipant = null;
                    metric.Score = 0;
                }
            }

            foreach (var participant in response.ParticipantResults)
            {
                var onlyParticipantRatings = ratingsInSpace.Where(e => e.RateeId == participant.ParticipantId).ToList();
                participant.AverageScore = onlyParticipantRatings.Any()
                    ? onlyParticipantRatings.Average(e => e.Score)
                    : 0;
                participant.MetricResults = metrics.Select(e =>
                {

                    var metricDto = _mapper.Map<ParticipantResultMetricDto>(e);
                    var metricRatings = onlyParticipantRatings.Where(r => r.MetricId == e.Id).ToList();
                    metricDto.averageMetricScore = metricRatings.Any() ? metricRatings.Average(r => r.Score) : 0;
                    return metricDto;

                }).ToList();
            }

            response.ParticipantResults = response.ParticipantResults.OrderByDescending(e => e.AverageScore).ToList();

            return response;
        }
    }
}
