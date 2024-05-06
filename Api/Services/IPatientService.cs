using Api.Dtos;

namespace Api.Services;

public interface IDoctorService
{
    IEnumerable<DoctorResponseDto> GetAll();
    Task<DoctorResponseDto> GetById(Guid id);
    Task<DoctorResponseDto> Insert(DoctorRequestDto dto);
    Task<DoctorResponseDto> Edit(Guid id, DoctorRequestDto dto);
    Task DeleteByIdAsync(Guid id);
}