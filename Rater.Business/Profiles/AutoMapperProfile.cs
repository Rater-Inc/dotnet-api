using AutoMapper;
using Rater.API;
using Rater.Domain.DataTransferObjects.MetricDto;
using Rater.Domain.DataTransferObjects.ParticipantDto;
using Rater.Domain.DataTransferObjects.RatingDto;
using Rater.Domain.DataTransferObjects.SpaceDto;
using Rater.Domain.DataTransferObjects.UserDto;

namespace Rater.Business.Profiles
{
    public class AutoMapperProfile : Profile 
    {
        public AutoMapperProfile()
        {

            //------------------------------------ SPACE DTO'S --------------------------------------------------------------


            CreateMap<Space, SpaceResponseDto>()
                .ForMember(dest => dest.SpaceId, opt => opt.MapFrom(src => src.SpaceId))
                .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.CreatorId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IsLocked, opt => opt.MapFrom(src => src.IsLocked))
                .ForMember(dest => dest.Link, opt => opt.MapFrom(src => src.Link))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.Metrics, opt => opt.MapFrom(src => src.Metrics))
                .ReverseMap();


            CreateMap<Space, SpaceRequestDto>()
                .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.CreatorId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IsLocked, opt => opt.MapFrom(src => src.IsLocked))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Metrics, opt => opt.MapFrom(src => src.Metrics))
                .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.Participants))
                .ReverseMap();

            CreateMap<Space, GrandSpaceRequestDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IsLocked, opt => opt.MapFrom(src => src.IsLocked))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Metrics, opt => opt.MapFrom(src => src.Metrics))
                .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.Participants))
                .ReverseMap();

            //------------------------------------ METRIC DTO'S --------------------------------------------------------------


            CreateMap<Metric, MetricResponseDto>()
                .ForMember(dest => dest.MetricId, opt => opt.MapFrom(src => src.MetricId))
                .ForMember(dest => dest.SpaceId, opt => opt.MapFrom(src => src.SpaceId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ReverseMap();

            CreateMap<Metric, MetricRequestDto>()
                .ForMember(dest => dest.SpaceId, opt => opt.MapFrom(src => src.SpaceId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ReverseMap();



            //------------------------------------ RATING DTO'S --------------------------------------------------------------


            CreateMap<Rating, RatingForMetricResponseDto>()
                .ForMember(dest => dest.RatingId, opt => opt.MapFrom(src => src.RatingId))
                .ForMember(dest => dest.RaterId, opt => opt.MapFrom(src => src.RaterId))
                .ForMember(dest => dest.RateeId, opt => opt.MapFrom(src => src.RateeId))
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score))
                .ForMember(dest => dest.RatedAt, opt => opt.MapFrom(src => src.RatedAt))
                .ForMember(dest => dest.Ratee, opt => opt.MapFrom(src => src.Ratee))
                .ForMember(dest => dest.Rater, opt => opt.MapFrom(src => src.Rater))
                .ReverseMap();



            CreateMap<Rating, RatingDetailDto>()
                .ForMember(dest => dest.RateeId, opt => opt.MapFrom(src => src.RateeId))
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score))
                .ForMember(dest => dest.MetricId, opt => opt.MapFrom(src => src.MetricId))
                .ReverseMap();





            //------------------------------------ PARTICIPANT DTO'S --------------------------------------------------------------

            CreateMap<Participant, ParticipantResponseDto>()
                .ForMember(dest => dest.ParticipantId, opt => opt.MapFrom(src => src.ParticipantId))
                .ForMember(dest => dest.ParticipantName, opt => opt.MapFrom(src => src.ParticipantName))
                .ReverseMap();

            CreateMap<Participant, ParticipantRequestDto>()
                .ForMember(dest => dest.SpaceId, opt => opt.MapFrom(src => src.SpaceId))
                .ForMember(dest => dest.ParticipantName, opt => opt.MapFrom(src => src.ParticipantName))
                .ReverseMap();

            //------------------------------------ User DTO'S --------------------------------------------------------------

            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Nickname, opt => opt.MapFrom(src => src.Nickname))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ReverseMap();

            CreateMap<User, UserRequestDto>()
                .ForMember(dest => dest.NickName, opt => opt.MapFrom(src => src.Nickname))
                .ReverseMap();





        }

    }
}
