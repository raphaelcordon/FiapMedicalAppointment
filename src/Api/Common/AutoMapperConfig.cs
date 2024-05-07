using Api.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Api.Common;

public class AutoMapperConfigurations : Profile
{
    public AutoMapperConfigurations()
    {
        // User Profiles
        CreateMap<UserProfile, UserProfileResponseDto>();
        CreateMap<UserRegisterDto, UserProfile>();
        CreateMap<UserUpdateDto, UserProfile>().ReverseMap();
    }
}