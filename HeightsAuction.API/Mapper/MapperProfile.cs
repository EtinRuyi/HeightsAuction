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
            CreateMap<PageResult<IEnumerable<AppUser>>, PageResult<IEnumerable<RegisterResponseDto>>>();
        }
    }
}
