namespace Domain.Entities;

public class Appointment : BaseEntity
{
    public DateTime AppointmentDateTime { get; set; }
    public Guid DoctorId { get; set; }
    public Guid PatientId { get; set; }
    public Guid AppointmentSpanId { get; set; }

    // Navigation properties
    public virtual UserProfile Doctor { get; set; }
    public virtual UserProfile Patient { get; set; }
    public virtual AppointmentSpan AppointmentSpan { get; set; }
}