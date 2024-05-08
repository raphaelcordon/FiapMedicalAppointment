using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User : IdentityUser<Guid>
{
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public virtual ICollection<MedicalSpecialty> MedicalSpecialties { get; set; } = new List<MedicalSpecialty>();
}