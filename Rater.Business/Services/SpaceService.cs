using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Rater.API;
using Rater.Business.Services.Interfaces;
using Rater.Data.Repositories.Interfaces;
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
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        public SpaceService(ISpaceRepository spaceRepo,IUserRepository userRepo,IMapper mapper)
        {
            _spaceRepo = spaceRepo;
            _userRepo = userRepo;
            _mapper = mapper;
        }



        public async Task<List<SpaceResponseDto>> GetAllSpaces()
        {
            var value = await _spaceRepo.GetAllSpaces();
            return value;
        }

        public async Task<SpaceResponseDto> AddSpace(GrandSpaceRequestDto request)
        {
            UserRequestDto userRequest = new UserRequestDto();
            userRequest.NickName = request.creatorNickname;
            var justCreatedUser = await _userRepo.AddUser(userRequest);


            var space = _mapper.Map<Space>(request);
            space.CreatorId = await _userRepo.GetCreatorId(justCreatedUser.CreatedAt);

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
    }
}
