using Domain.Dtos;

namespace Domain.Interfaces.Services;

public interface IMedicalSpecialtyService
{
    Task<IEnumerable<MedicalSpecialtiesDtos.MedicalSpecialtyDto>> GetAllSpecialties();
    Task<MedicalSpecialtiesDtos.MedicalSpecialtyDto> GetSpecialtyById(Guid id);
    Task<MedicalSpecialtiesDtos.MedicalSpecialtyDto> AddSpecialty(MedicalSpecialtiesDtos.CreateMedicalSpecialtyDto createDto);
    Task<MedicalSpecialtiesDtos.MedicalSpecialtyDto> UpdateSpecialty(Guid id, MedicalSpecialtiesDtos.UpdateMedicalSpecialtyDto updateDto);
    Task DeleteSpecialty(Guid id);
    Task AddSpecialtyToDoctor(Guid userId, Guid specialtyId);
}