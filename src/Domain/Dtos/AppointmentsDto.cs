namespace Domain.Dtos;


    public class ScheduleAppointmentDto
    {
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }
        public Guid SpecialtyId { get; set; }
        public DateTime AppointmentTime { get; set; }
        public Guid SpanId { get; set; }
    }

    public class AppointmentDto
    {
        public Guid Id { get; set; }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public DateTime AppointmentTime { get; set; }
        public int DurationMinutes { get; set; }
        public string Specialty { get; set; }
        public string Status { get; set; }
    }

    public class UpdateAppointmentDto
    {
        public DateTime? NewAppointmentTime { get; set; }
        public int? NewSpan { get; set; }
        public string NewStatus { get; set; }
    }