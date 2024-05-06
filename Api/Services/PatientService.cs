using System.Data;
using Api.Common;
using Api.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Api.Services;

public class DoctorService : IDoctorService
{

    private readonly IBaseRepository<Doctor> _repository;
    private readonly IMapper _mapper;

    public DoctorService(IBaseRepository<Doctor> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public IEnumerable<DoctorResponseDto> GetAll()
    {
        var doctors = _repository.List().ToArray();
        return _mapper.Map<IEnumerable<DoctorResponseDto>>(doctors);
    }

    public async Task<DoctorResponseDto> GetById(Guid id)
    {
        var doctor = await _repository.FindAsync(id);
        if (doctor is null)
            throw new ResourceNotFoundException($"No doctor found with the id: {id}");

        return _mapper.Map<DoctorResponseDto>(doctor);
    }

    public async Task<DoctorResponseDto> Insert(DoctorRequestDto dto)
    {
        var doctor = _mapper.Map<Doctor>(dto);
        await _repository.AddAsync(doctor);
        await _repository.SaveChangesAsync();
        return _mapper.Map<DoctorResponseDto>(doctor);
    }

    public async Task<DoctorResponseDto> Edit(Guid id, DoctorRequestDto dto)
    {
        var doctor = await _repository.FindAsync(id);
        if (doctor is null)
            throw new ReadOnlyException($"No doctor found with the id: {id}");
        
        // TO BE IMPLEMENTED
        // doctor.UpdateDetails();
        // var savedDoctor = _repository.Update(doctor);
        // await _repository.SaveChangesAsync();

        // return _mapper.Map<DoctorResponseDto>(savedDoctor);
        return _mapper.Map<DoctorResponseDto>(dto);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var doctor = await _repository.FindAsync(id);
        if (doctor is null)
            throw new ResourceNotFoundException($"No doctor was found with the id: {id}");
		
        await _repository.DeleteAsync(id);
        await _repository.SaveChangesAsync();
    }
}