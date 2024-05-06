namespace Domain.Entities;

public class Doctor
{
    public User User { get; set; }
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ICollection<MedicalSpecialty> MedicalSpecialties { get; set; } = new List<MedicalSpecialty>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    
    public void UpdateDetails(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}