using AutoMapper;
using HeightsAuction.Application.DTOs;
using HeightsAuction.Common.Utilities;
using HeightsAuction.Domain.Entities;

namespace HeightsAuction.API.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<AppUser, RegisterResponseDto>().ReverseMap();
            CreateMap<RegisterRequestDto, AppUser>();
            CreateMap<AppUser, LoginResponseDto>().ReverseMap();
            CreateMap<LoginRequestDto, AppUser>();
            CreateMap<CreateRoomRequestDto, BiddingRoom>();
            CreateMap<BiddingRoom, CreateRoomResponseDto>().ReverseMap();
            CreateMap<PageResult<IEnumerable<AppUser>>, PageResult<IEnumerable<RegisterResponseDto>>>();
            CreateMap<BiddingRoom, BiddingRoomDto>()
                .ForMember(dest => dest.Bidders, opt => opt.MapFrom(src => src.Bidders))
                .ReverseMap();
            CreateMap<BiddingRoom, JoinRoomResponseDto>()
                .ForMember(dest => dest.Bidders, opt => opt.MapFrom(src => src.Bidders))
                .ReverseMap();
            CreateMap<BiddingRoom, CreateRoomResponseDto>().ReverseMap();
            CreateMap<Bid, AddBidResponseDto>().ReverseMap();
            CreateMap<AddBidRequestDto, Bid>();
        }
    }
}
