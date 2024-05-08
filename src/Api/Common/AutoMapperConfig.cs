using Domain.Dtos;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Api.Common;

public class AutoMapperConfigurations : Profile
{
    public AutoMapperConfigurations()
    {
        // User Profiles
        CreateMap<UserProfile, UserProfileResponseDto>()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom<UserRolesResolver>());

        CreateMap<UserRegisterDto, UserProfile>();
        CreateMap<UserUpdateDto, UserProfile>().ReverseMap();
        
        // Medial Specialties Profiles
        CreateMap<MedicalSpecialty, MedicalSpecialtiesDtos.MedicalSpecialtyDto>();
        CreateMap<MedicalSpecialtiesDtos.CreateMedicalSpecialtyDto, MedicalSpecialty>();
    }
    
}