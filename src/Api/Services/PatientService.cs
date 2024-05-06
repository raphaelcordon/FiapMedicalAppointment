using System.Data;
using Api.Common;
using Api.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Api.Services;

public class PatientService : IPatientService
{
    private readonly IBaseRepository<Patient> _repository;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public PatientService(IBaseRepository<Patient> repository, UserManager<User> userManager, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public IEnumerable<PatientResponseDto> GetAll()
    {
        var patients = _repository.List().ToArray();
        return _mapper.Map<IEnumerable<PatientResponseDto>>(patients);
    }

    public async Task<PatientResponseDto> GetById(Guid id)
    {
        var patient = await _repository.FindAsync(id);
        if (patient is null)
            throw new ResourceNotFoundException($"No patient found with the id: {id}");

        return _mapper.Map<PatientResponseDto>(patient);
    }

    public async Task<PatientResponseDto> Insert(PatientRequestDto dto)
    {
        using (var transaction = _repository.BeginTransaction())
        {
            try
            {
                var user = new User
                {
                    UserName = dto.UserDto.UserName,
                    Email = dto.UserDto.Email
                };
                var createUserResult = await _userManager.CreateAsync(user, dto.UserDto.Password);
                if (!createUserResult.Succeeded)
                {
                    throw new Exception("User creation failed");
                }

                var patient = _mapper.Map<Patient>(dto);
                patient.UserId = user.Id;
                await _repository.AddAsync(patient);
                await _repository.SaveChangesAsync();

                transaction.Commit();
                return _mapper.Map<PatientResponseDto>(patient);
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }

    public async Task<PatientResponseDto> Edit(Guid id, PatientRequestDto dto)
    {
        var patient = await _repository.FindAsync(id);
        if (patient == null)
        {
            throw new ReadOnlyException($"No patient found with the id: {id}");
        }
        
        _mapper.Map(dto, patient);
        _repository.Update(patient);
        await _repository.SaveChangesAsync();
        return _mapper.Map<PatientResponseDto>(patient);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var patient = await _repository.FindAsync(id);
        if (patient is null)
            throw new ResourceNotFoundException($"No patient was found with the id: {id}");
		
        await _repository.DeleteAsync(id);
        await _repository.SaveChangesAsync();
    }
}