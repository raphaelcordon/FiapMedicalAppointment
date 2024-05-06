namespace Domain.Entities;

public class AppointmentSpan : BaseEntity
{
    public int Duration { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}