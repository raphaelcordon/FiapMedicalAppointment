using Domain.Entities;

namespace Api.Dtos;

public class PatientResponseDto
{
    public User User { get; set; }
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Cpf { get; set; }
    public string Address { get; set; }
    public string Zip { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}