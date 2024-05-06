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
            // Create the user
            var user = new User
            {
                UserName = dto.UserDto.UserName,
                Email = dto.UserDto.Email
            };
            var createUserResult = await _userManager.CreateAsync(user, dto.UserDto.Password);
            if (!createUserResult.Succeeded)
            {
                throw new Exception("User creation failed: " + createUserResult.Errors.FirstOrDefault()?.Description);
            }

            // Assign role to the user
            var addToRoleResult = await _userManager.AddToRoleAsync(user, "Doctor"); // Assuming role is correctly named as "Doctor"
            if (!addToRoleResult.Succeeded)
            {
                throw new Exception("Adding user to role failed: " + addToRoleResult.Errors.FirstOrDefault()?.Description);
            }

            // Create doctor-specific details
            var doctor = _mapper.Map<Doctor>(dto); // Assuming dto contains doctor-specific data
            doctor.Id = user.Id; // Link the Doctor entity with the User ID
            await _repository.AddAsync(doctor);
            await _repository.SaveChangesAsync();

            transaction.Commit();
            return _mapper.Map<DoctorResponseDto>(doctor);
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new Exception("An error occurred while creating the doctor: " + ex.Message, ex);
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