using AutoMapper;
using Rater.API;
using Rater.Domain.DataTransferObjects.SpaceDto;

namespace Rater.Business.Profiles
{
    internal class AutoMapperProfile : Profile 
    {
        public AutoMapperProfile()
        {

            CreateMap<Space, SpaceResponseDto>()
                .ForMember(dest => dest.SpaceId, opt => opt.MapFrom(src => src.SpaceId))
                .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.CreatorId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IsLocked, opt => opt.MapFrom(src => src.IsLocked))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Link, opt => opt.MapFrom(src => src.Link))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ReverseMap();

            
        }

    }
}
