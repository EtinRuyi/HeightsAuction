using AutoMapper;
using HeightsAuction.Application.DTOs;
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
        }
    }
}
