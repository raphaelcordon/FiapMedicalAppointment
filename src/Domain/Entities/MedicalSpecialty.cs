namespace Domain.Entities;

public class MedicalSpecialty : BaseEntity
{
    public string Specialty { get; set; }
    public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
}