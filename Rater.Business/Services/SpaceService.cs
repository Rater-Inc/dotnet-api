using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Rater.API;
using Rater.Business.Services.Interfaces;
using Rater.Data.Repositories.Interfaces;
using Rater.Domain.DataTransferObjects.MetricDto;
using Rater.Domain.DataTransferObjects.ParticipantDto;
using Rater.Domain.DataTransferObjects.ResultDto;
using Rater.Domain.DataTransferObjects.SpaceDto;
using Rater.Domain.DataTransferObjects.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Business.Services
{
    public class SpaceService : ISpaceService
    {

        private readonly ISpaceRepository _spaceRepo;
        private readonly IUserService _userService;
        private readonly IRatingService _ratingService;
        private readonly IMapper _mapper;
        private readonly IJwtTokenService _tokenService;
        private readonly IMetricService _metricService;
        public SpaceService(
            ISpaceRepository spaceRepo,
            IUserService userService,
            IMetricService metricService,
            IRatingService ratingService,
            IMapper mapper,
            IJwtTokenService tokenService)
        {
            _spaceRepo = spaceRepo;
            _userService = userService;
            _metricService = metricService;
            _ratingService = ratingService;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<SpaceResponseDto> AddSpace(GrandSpaceRequestDto request)
        {
            UserRequestDto userRequest = new UserRequestDto();
            userRequest.NickName = request.creatorNickname;
            var justCreatedUser = await _userService.CreateUser(userRequest);


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

            var finalRequest = _mapper.Map<SpaceRequestDto>(space);
            var result = await _spaceRepo.CreateSpace(finalRequest);

            return result;
        }



        public async Task<SpaceResponseDto> GetSpace(string link)
        {

            try
            {
                var value = await _spaceRepo.GetSpaceByLink(link);
                if (_tokenService.GetSpaceIdFromToken() != value.SpaceId)
                {
                    throw new UnauthorizedAccessException("Unauthorized for this space");
                }
                var returner = _mapper.Map<SpaceResponseDto>(value);
                return returner;
            }
            catch(UnauthorizedAccessException ex)
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
                if(_tokenService.GetSpaceIdFromToken() != space.SpaceId)
                {
                    throw new UnauthorizedAccessException("Unauthorized for this space");
                }

                GrandResultResponseDto response = new GrandResultResponseDto();
                response.SpaceId = space.SpaceId;
                response.Name = space.Name;

                var metrics = await _metricService.GetMetrics(space.SpaceId);
                var metricResponse = metrics.Select(e => _mapper.Map<MetricResponseForResultDto>(e)).ToList();
                response.Metrics = metricResponse;

                var ratingsInSpace = await _ratingService.GetRatings(space.SpaceId);

                foreach (var metric in response.Metrics)
                {
                    var metricRatings = ratingsInSpace.Where(e => e.MetricId == metric.Id).ToList();

                    if(metricRatings.Any())
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

                        metric.Winner = _mapper.Map<ParticipantResponseDto>(averageScore?.Ratee);
                        metric.Score = averageScore?.AverageScore ?? 0;
                    }
                    else
                    {
                        metric.Winner = null;
                        metric.Score = 0;   
                    }
                }

                return response;
            }

            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException(ex.Message);
            }

            catch (Exception ex) {

                throw new Exception(ex.Message);
            
            }



        }




    }
}
