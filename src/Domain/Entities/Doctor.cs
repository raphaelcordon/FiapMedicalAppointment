namespace Domain.Entities;

public class Doctor : User
{
    public ICollection<MedicalSpecialty> MedicalSpecialties { get; set; } = new List<MedicalSpecialty>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}