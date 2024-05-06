namespace Domain.Entities;

public class Patient : User
{
    public string Cpf { get; set; }
    public string Address { get; set; }
    public string Zip { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}