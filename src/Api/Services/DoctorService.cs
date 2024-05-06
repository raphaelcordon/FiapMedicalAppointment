using System.Data;
using Api.Common;
using Api.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Api.Services;

public class DoctorService : IDoctorService
{

    private readonly IBaseRepository<Doctor> _repository;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public DoctorService(IBaseRepository<Doctor> repository, UserManager<User> userManager,IMapper mapper)
    {
        _repository = repository;
        _userManager = userManager;
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
        if (doctor == null)
            throw new ResourceNotFoundException($"No doctor found with the id: {id}");

        return _mapper.Map<DoctorResponseDto>(doctor);
    }

    public async Task<DoctorResponseDto> Insert(DoctorRequestDto dto)
    {
        using var transaction = _repository.BeginTransaction();
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

            var doctor = _mapper.Map<Doctor>(dto);
            doctor.UserId = user.Id;
            await _repository.AddAsync(doctor);
            await _repository.SaveChangesAsync();

            transaction.Commit();
            return _mapper.Map<DoctorResponseDto>(doctor);
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<DoctorResponseDto> Edit(Guid id, DoctorRequestDto dto)
    {
        var doctor = await _repository.FindAsync(id);
        if (doctor == null)
        {
            throw new ReadOnlyException($"No doctor found with the id: {id}");
        }
        
        _mapper.Map(dto, doctor);
        _repository.Update(doctor);
        await _repository.SaveChangesAsync();
        return _mapper.Map<DoctorResponseDto>(doctor);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var doctor = await _repository.FindAsync(id);
        if (doctor == null)
            throw new ResourceNotFoundException($"No doctor was found with the id: {id}");
		
        await _repository.DeleteAsync(id);
        await _repository.SaveChangesAsync();
    }
}