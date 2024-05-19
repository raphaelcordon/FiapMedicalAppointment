using Domain.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Api.Common
{
    public class AutoMapperConfigurations : Profile
    {
        public AutoMapperConfigurations()
        {
            // User Profiles
            CreateMap<UserProfile, UserProfileResponseDto>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom<UserRolesResolver>())
                .ForMember(dest => dest.MedicalSpecialties, opt => opt.MapFrom(src => src.MedicalSpecialties.Select(ms => ms.Specialty).ToList()));
            
            CreateMap<UserRegisterDto, UserProfile>();
            CreateMap<UserUpdateDto, UserProfile>().ReverseMap();
            
            // Medical Specialties Profiles
            CreateMap<MedicalSpecialty, MedicalSpecialtiesDtos.MedicalSpecialtyDto>();
            CreateMap<MedicalSpecialtiesDtos.CreateMedicalSpecialtyDto, MedicalSpecialty>();
        }
    }
}