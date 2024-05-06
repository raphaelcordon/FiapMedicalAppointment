using Api.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Api.Common;

public class AutoMapperConfigurations : Profile
{
    public AutoMapperConfigurations()
    {
        // Doctor
        CreateMap<Doctor, DoctorRequestInsertDto>();
        CreateMap<DoctorRequestInsertDto, Doctor>();
        CreateMap<Doctor, DoctorResponseDto>();
        CreateMap<DoctorResponseDto, Doctor>();
        CreateMap<Doctor, DoctorRequestDto>();
        CreateMap<DoctorRequestDto, Doctor>();
        
        // Patient
        CreateMap<Patient, PatientRequestInsertDto>();
        CreateMap<PatientRequestInsertDto, Patient>();
        CreateMap<Patient, PatientResponseDto>();
        CreateMap<PatientResponseDto, Patient>();
        CreateMap<Patient, PatientRequestDto>();
        CreateMap<PatientRequestDto, Patient>();
    }
}