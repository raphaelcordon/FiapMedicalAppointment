using Domain.Interfaces;

namespace Domain.Entities;

public class UserProfile : User, ISoftDeletable
{
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<MedicalSpecialty> MedicalSpecialties { get; set; } = new List<MedicalSpecialty>();
}