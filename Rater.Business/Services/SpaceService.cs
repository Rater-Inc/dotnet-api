using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RandomString4Net;
using Rater.API;
using Rater.Business.Services.Interfaces;
using Rater.Data.Repositories.Interfaces;
using Rater.Domain.DataTransferObjects.MetricDto;
using Rater.Domain.DataTransferObjects.ParticipantDto;
using Rater.Domain.DataTransferObjects.RatingDto;
using Rater.Domain.DataTransferObjects.ResultDto;
using Rater.Domain.DataTransferObjects.SpaceDto;
using Rater.Domain.DataTransferObjects.UserDto;

namespace Rater.Business.Services
{
    public class SpaceService : ISpaceService
    {

        private readonly ISpaceRepository _spaceRepo;
        private readonly IUserService _userService;
        private readonly IRatingService _ratingService;
        private readonly IParticipantService _participantService;
        private readonly IMapper _mapper;
        private readonly IMetricService _metricService;
        public SpaceService(
            ISpaceRepository spaceRepo,
            IUserService userService,
            IMetricService metricService,
            IRatingService ratingService,
            IParticipantService participantService,
            IMapper mapper)
        {
            _spaceRepo = spaceRepo;
            _userService = userService;
            _metricService = metricService;
            _ratingService = ratingService;
            _participantService = participantService;
            _mapper = mapper;
        }

        public async Task<SpaceResponseDto> AddSpace(GrandSpaceRequestDto request)
        {
            var justCreatedUser = await _userService.CreateUser(new UserRequestDto { NickName = request.creatorNickname });
            var space = _mapper.Map<Space>(request);
            space.CreatorId = justCreatedUser.UserId;

            foreach (var metrics in space.Metrics)
            {
                metrics.SpaceId = space.SpaceId;
            }

            foreach (var participants in space.Participants)
            {
                participants.SpaceId = space.SpaceId;
            }

            space.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            space.Link =  RandomString.GetString(Types.ALPHANUMERIC_LOWERCASE);

            var result = await _spaceRepo.CreateSpace(space);

            return result;
        }



        public async Task<SpaceResponseDto> GetSpace(string link)
        {

            try
            {
                var value = await _spaceRepo.GetSpaceByLink(link);
                var returner = _mapper.Map<SpaceResponseDto>(value);
                return returner;
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


        public async Task<GrandResultResponseDto> GetSpaceResults(string link)
        {

            try
            {
                var space = await _spaceRepo.GetSpaceByLink(link);

                if (space == null)
                    throw new InvalidOperationException("Space not found for the given link.");

                GrandResultResponseDto response = new GrandResultResponseDto();
                response.SpaceId = space.SpaceId;
                response.Name = space.Name;

                var metrics = await _metricService.GetMetrics(space.SpaceId);
                var metricResponse = metrics.Select(e => _mapper.Map<MetricLeaderDto>(e)).ToList();
                response.MetricLeaders = metricResponse;

                var participants = await _participantService.GetParticipants(space.SpaceId);
                var participantResponse = participants.Select(e => _mapper.Map<PariticipantResultDto>(e)).ToList();
                response.ParticipantResults = participantResponse;

                var ratingsInSpace = await _ratingService.GetRatings(space.SpaceId);

                foreach (var metric in response.MetricLeaders)
                {
                    var metricRatings = ratingsInSpace.Where(e => e.MetricId == metric.Id).ToList();

                    if (metricRatings.Any())
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
                        var metricRatings = onlyParticipantRatings.Where(r => r.MetricId == e.MetricId).ToList();
                        metricDto.averageMetricScore = metricRatings.Any() ? metricRatings.Average(r => r.Score) : 0;
                        return metricDto;

                    }).ToList();
                }

                response.ParticipantResults = response.ParticipantResults.OrderByDescending(e => e.AverageScore).ToList();
                return response;
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RatingResponseDto> AddRatings(RatingRequestDto request)
        {
            try
            {
                var metricIds = request.RatingDetails.Select(x => x.MetricId).ToList();
                var participantIds = request.RatingDetails.Select(x => x.RateeId).ToList();

                var metrics = await _metricService.GetMetricsGivenIds(metricIds);
                var participants = await _participantService.GetParticipantsGivenIds(participantIds);

                var metricsDict = metrics.ToDictionary(m => m.MetricId);
                var participantsDict = participants.ToDictionary(p => p.ParticipantId);

                foreach (var x in request.RatingDetails)
                {
                    if (!metricsDict.TryGetValue(x.MetricId, out var metric) || metric.SpaceId != request.SpaceId)
                    {
                        throw new InvalidOperationException("The request payload does not match the provided space ID for metric.");
                    }

                    if (!participantsDict.TryGetValue(x.RateeId, out var participant) || participant.SpaceId != request.SpaceId)
                    {
                        throw new InvalidOperationException("The request payload does not match the provided space ID for participant.");
                    }
                }

                var user = await _userService.CreateUser(new UserRequestDto { NickName = request.RaterNickName });

                var ratings = request.RatingDetails.Select(e =>
                {
                    var rating = _mapper.Map<Rating>(e);
                    rating.RaterId = user.UserId;
                    rating.SpaceId = request.SpaceId;
                    return rating;

                }).ToList();

                var returner = await _ratingService.AddRatings(ratings);
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

        public async Task<bool> SpaceExist(int space_id)
        {
            var result = await _spaceRepo.SpaceExist(space_id);
            return result;
        }

        public async Task<Space?> GetSpaceByLink(string link)
        {
            var result = await _spaceRepo.GetSpaceByLink(link);
            return result;
        }
    }
}
