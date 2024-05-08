using System;
using Domain.Interfaces;

namespace Domain.Entities;

public class Appointment : BaseEntity, ISoftDeletable
{
    public DateTime AppointmentDateTime { get; set; }
    public Guid DoctorId { get; set; }
    public Guid PatientId { get; set; }
    public Guid AppointmentSpanId { get; set; }
    public string Status { get; set; }
    
    // ISoftDeletable Implementation
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual UserProfile Doctor { get; set; }
    public virtual UserProfile Patient { get; set; }
    public virtual AppointmentSpan AppointmentSpan { get; set; }
    public Guid SpecialtyId { get; set; }
    public virtual MedicalSpecialty Specialty { get; set; }
}
