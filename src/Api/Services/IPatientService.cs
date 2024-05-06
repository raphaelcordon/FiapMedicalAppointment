using Api.Dtos;

namespace Api.Services;

public interface IPatientService
{
    IEnumerable<PatientResponseDto> GetAll();
    Task<PatientResponseDto> GetById(Guid id);
    Task<PatientResponseDto> Insert(PatientRequestDto dto);
    Task<PatientResponseDto> Edit(Guid id, PatientRequestDto dto);
    Task DeleteByIdAsync(Guid id);
}