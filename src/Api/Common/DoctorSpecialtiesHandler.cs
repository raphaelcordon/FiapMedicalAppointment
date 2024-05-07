using Domain.Entities;
using Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Api.Common;

public class DoctorSpecialtiesHandler
{
    private readonly UserManager<User> _userManager;
    private readonly DatabaseContext _context;

    public DoctorSpecialtiesHandler(UserManager<User> userManager, DatabaseContext context)
    {
        _userManager = userManager;
        _context = context;
    }
    
    public async Task AddSpecialtyToDoctor(string userId, string specialtyName)
    {
        var user = await _userManager.FindByIdAsync(userId) as UserProfile;
        if (user == null || !await _userManager.IsInRoleAsync(user, "Doctor"))
            throw new Exception("User is not a doctor or doesn't exist.");

        var specialty = await _context.MedicalSpecialties.FirstOrDefaultAsync(s => s.Specialty == specialtyName);
        if (specialty == null)
        {
            specialty = new MedicalSpecialty { Specialty = specialtyName };
            _context.MedicalSpecialties.Add(specialty);
        }

        user.MedicalSpecialties.Add(specialty);
        await _context.SaveChangesAsync();
    }
}