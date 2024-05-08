using Domain.Entities;
using Domain.Interfaces;
using Api.Common;
using Domain.Dtos;
using AutoMapper;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Identity;

namespace Api.Services;
public class MedicalSpecialtyService : IMedicalSpecialtyService
{
    private readonly IBaseRepository<MedicalSpecialty> _specialtyRepository;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly UserManager<User> _userManager;

    public MedicalSpecialtyService(IBaseRepository<MedicalSpecialty> specialtyRepository, IMapper mapper, IUserService userService, UserManager<User> userManager)
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
        var addedSpecialty = await _specialtyRepository.AddAsync(specialty);
        await _specialtyRepository.SaveChangesAsync();
        return _mapper.Map<MedicalSpecialtiesDtos.MedicalSpecialtyDto>(addedSpecialty);
    }

    public async Task<MedicalSpecialtiesDtos.MedicalSpecialtyDto> UpdateSpecialty(Guid id, MedicalSpecialtiesDtos.UpdateMedicalSpecialtyDto updateDto)
    {
        var specialty = await _specialtyRepository.FindAsync(id);
        if (specialty == null)
            throw new ResourceNotFoundException("Specialty not found.");

        specialty.Specialty = updateDto.Specialty;
        _specialtyRepository.Update(specialty);
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

        // Ensure the user is a doctor
        if (!await _userManager.IsInRoleAsync(user, "Doctor"))
            throw new Exception("User is not a doctor.");

        // Fetch the specialty and check if it exists
        var specialty = await _specialtyRepository.FindAsync(specialtyId);
        if (specialty == null)
            throw new Exception("Specialty not found.");

        // Check if the doctor already has this specialty
        if (user.MedicalSpecialties.Any(ms => ms.Id == specialtyId))
            throw new Exception("Doctor already has this specialty.");

        // Add the specialty to the doctor's list
        user.MedicalSpecialties.Add(specialty);
        await _userManager.UpdateAsync(user); // Persist changes
    }
}