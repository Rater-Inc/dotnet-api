using AutoMapper;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Rater.API;
using Rater.Business.Services.Interfaces;
using Rater.Data.Repositories.Interfaces;
using Rater.Domain.DataTransferObjects.RatingDto;
using Rater.Domain.DataTransferObjects.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Rater.Business.Services
{
    public class RatingService : IRatingService
    {


        private readonly IRatingRepository _repo;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public RatingService(IRatingRepository repo,IUserService userService,IMapper mapper)
        {
            _repo = repo;
            _userService = userService;
            _mapper = mapper;
        }


        public async Task<List<RatingForMetricResponseDto>> AddRatings(RatingRequestDto request)
        {

            if(request.RatingDetails.Count == 0 || request.RatingDetails == null)
            {
                throw new Exception("Bad Request of Ratings");                                       // If rating request is empty throw exception
            } 

            if(request.RaterNickName == "")
            {
                throw new Exception("The nickname request is either empty or problematic.");        // If nickname is empty
            }

            UserRequestDto userRequest = new UserRequestDto();
            userRequest.NickName = request.RaterNickName;
            var user = await _userService.CreateUser(userRequest);

            var ratings = request.RatingDetails.Select(e => _mapper.Map<Rating>(e)).ToList();

            foreach(var x in ratings)
            {
                x.RaterId = user.UserId;
                x.SpaceId = request.SpaceId;
            }

            try
            {
                var returner = await _repo.AddRatings(ratings);
                return returner;
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);                                                    //Forwarding the exception
            }




        }

    }
}
