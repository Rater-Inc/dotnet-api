using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Rater.API;
using Rater.Business.Services.Interfaces;
using Rater.Data.Repositories.Interfaces;
using Rater.Domain.DataTransferObjects.RatingDto;
using Rater.Domain.DataTransferObjects.UserDto;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Business.Services
{
    public class RatingService : IRatingService
    {

        private readonly IRatingRepository _repo;
        private readonly IUserService _userService;
        private readonly IJwtTokenService _tokenService;
        private readonly IMapper _mapper;
        public RatingService(
            IRatingRepository repo,
            IUserService userService,
            IMapper mapper,
            IJwtTokenService tokenService)
        {
            _repo = repo;
            _userService = userService;
            _mapper = mapper;
            _tokenService = tokenService;
        }


        public async Task<List<RatingForMetricResponseDto>> AddRatings(RatingRequestDto request)
        {

            try
            {
                if (_tokenService.GetSpaceIdFromToken() != request.SpaceId)
                {
                    throw new UnauthorizedAccessException("Unauthorized for this space");
                }

                UserRequestDto userRequest = new UserRequestDto();
                userRequest.NickName = request.RaterNickName;
                var user = await _userService.CreateUser(userRequest);

                var ratings = request?.RatingDetails?.Select(e => _mapper.Map<Rating>(e)).ToList();

                if (ratings == null) throw new Exception("Rating values are empty");

                foreach (var x in ratings)
                {
                    x.RaterId = user.UserId;
                    x.SpaceId = request.SpaceId;
                }

                var returner = await _repo.AddRatings(ratings);
                return returner;

            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException(ex.Message);
            }

            catch (Exception ex) {

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
