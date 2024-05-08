using Domain.Interfaces;

namespace Domain.Entities;

public class AppointmentSpan : BaseEntity, ISoftDeletable
{
    public int Duration { get; set; }
    public bool IsActive { get; set; } = true; 
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}