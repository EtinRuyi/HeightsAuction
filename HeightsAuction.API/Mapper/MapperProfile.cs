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
            CreateMap<PageResult<IEnumerable<AppUser>>, PageResult<IEnumerable<RegisterResponseDto>>>();


            CreateMap<BiddingRoom, CreateRoomResponseDto>().ReverseMap();
            CreateMap<CreateRoomRequestDto, BiddingRoom>();
            CreateMap<BiddingRoom, BiddingRoomDto>()
                .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Bids, opt => opt.MapFrom(src => src.Bids))
                .ForMember(dest => dest.Bidders, opt => opt.MapFrom(src => src.Bidders)).ReverseMap();
            CreateMap<BiddingRoom, JoinRoomResponseDto>()
                .ForMember(dest => dest.Bidders, opt => opt.MapFrom(src => src.Bidders))
                .ReverseMap();
            CreateMap<PageResult<IEnumerable<BiddingRoom>>, PageResult<IEnumerable<BiddingRoomDto>>>();


            CreateMap<Item, CreateItemResponseDto>().ReverseMap();
            CreateMap<CreateItemRequestDto, Item>();
            CreateMap<Item, ItemResponseDto>().ReverseMap();
            CreateMap<Item, ItemResponseDto>()
            .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.Id));
            CreateMap<PageResult<IEnumerable<Item>>, PageResult<IEnumerable<ItemResponseDto>>>();


            CreateMap<Bid, BidResponseDto>().ReverseMap();
            CreateMap<Bid, BidResponseDto>()
                .ForMember(dest => dest.BidId, opt => opt.MapFrom(src => src.Id)).ReverseMap();
            CreateMap<AddBidRequestDto, Bid>();















            CreateMap<GenerateInvoiceRequestDto, Invoice>();
            CreateMap<Invoice, GenerateInvoiceResponseDto>().ReverseMap();
            CreateMap<BidNotificationRequestDto, BidNotification>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<BidNotification, BidNotificationResponseDto>();
        }
    }
}
