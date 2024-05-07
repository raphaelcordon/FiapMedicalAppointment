namespace Domain.Entities;

public class MedicalSpecialty : BaseEntity
{
    public string Specialty { get; set; }
    public ICollection<UserProfile> Users { get; set; } = new List<UserProfile>();
}