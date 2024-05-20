using Api.Common;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class MedicalSpecialtyService : IMedicalSpecialtyService
{
    private readonly IBaseRepository<MedicalSpecialty> _specialtyRepository;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly UserManager<UserProfile> _userManager;

    public MedicalSpecialtyService(IBaseRepository<MedicalSpecialty> specialtyRepository, IMapper mapper, IUserService userService, UserManager<UserProfile> userManager)
    {
        _specialtyRepository = specialtyRepository;
        _mapper = mapper;
        _userService = userService;
        _userManager = userManager;
    }

    public async Task<IEnumerable<MedicalSpecialtiesDtos.MedicalSpecialtyDto>> GetAllSpecialties()
    {
        var specialties = await _specialtyRepository.ListAsync();
        return _mapper.Map<IEnumerable<MedicalSpecialtiesDtos.MedicalSpecialtyDto>>(specialties);
    }

    public async Task<MedicalSpecialtiesDtos.MedicalSpecialtyDto> GetSpecialtyById(Guid id)
    {
        var specialty = await _specialtyRepository.FindAsync(id);
        if (specialty == null)
            throw new ResourceNotFoundException("Specialty not found.");
        return _mapper.Map<MedicalSpecialtiesDtos.MedicalSpecialtyDto>(specialty);
    }

    public async Task<MedicalSpecialtiesDtos.MedicalSpecialtyDto> AddSpecialty(MedicalSpecialtiesDtos.CreateMedicalSpecialtyDto createDto)
    {
        var specialty = _mapper.Map<MedicalSpecialty>(createDto);
        await _specialtyRepository.AddAsync(specialty);
        await _specialtyRepository.SaveChangesAsync();
        return _mapper.Map<MedicalSpecialtiesDtos.MedicalSpecialtyDto>(specialty);
    }

    public async Task<MedicalSpecialtiesDtos.MedicalSpecialtyDto> UpdateSpecialty(Guid id, MedicalSpecialtiesDtos.UpdateMedicalSpecialtyDto updateDto)
    {
        var specialty = await _specialtyRepository.FindAsync(id);
        if (specialty == null)
            throw new ResourceNotFoundException("Specialty not found.");

        specialty.Specialty = updateDto.Specialty;
        await _specialtyRepository.UpdateAsync(specialty);
        await _specialtyRepository.SaveChangesAsync();
        return _mapper.Map<MedicalSpecialtiesDtos.MedicalSpecialtyDto>(specialty);
    }

    public async Task DeleteSpecialty(Guid id)
    {
        await _specialtyRepository.DeleteAsync(id);
        await _specialtyRepository.SaveChangesAsync();
    }

    public async Task AddSpecialtyToDoctor(Guid userId, Guid specialtyId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new Exception("User not found.");

        if (!await _userManager.IsInRoleAsync(user, "Doctor"))
            throw new Exception("User is not a doctor.");

        var specialty = await _specialtyRepository.FindAsync(specialtyId);
        if (specialty == null)
            throw new Exception("Specialty not found.");

        if (user.MedicalSpecialties.Any(ms => ms.Id == specialtyId))
            throw new Exception("Doctor already has this specialty.");

        user.MedicalSpecialties.Add(specialty);
        await _userManager.UpdateAsync(user);
    }

    public async Task RemoveSpecialtyFromDoctor(Guid userId, Guid specialtyId)
    {
        var user = await _userManager.Users
            .Include(u => u.MedicalSpecialties)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new Exception("User not found.");

        if (!await _userManager.IsInRoleAsync(user, "Doctor"))
            throw new Exception("User is not a doctor.");

        var specialty = user.MedicalSpecialties.FirstOrDefault(ms => ms.Id == specialtyId);
        if (specialty == null)
            throw new Exception("Doctor does not have this specialty.");

        user.MedicalSpecialties.Remove(specialty);
        await _userManager.UpdateAsync(user);
    }
}
