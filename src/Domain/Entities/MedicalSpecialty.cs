using Domain.Interfaces;

namespace Domain.Entities;

public class MedicalSpecialty : BaseEntity, ISoftDeletable
{
    public string Specialty { get; set; }
    public bool IsActive { get; set; } = true;
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}