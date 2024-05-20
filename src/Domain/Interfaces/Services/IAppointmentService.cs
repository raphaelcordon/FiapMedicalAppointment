using Domain.Dtos;

namespace Domain.Interfaces.Services;

public interface IAppointmentService
{
    Task<AppointmentDto> ScheduleAppointmentAsync(ScheduleAppointmentDto scheduleDto);
    Task<IEnumerable<AppointmentDto>> GetAppointmentsForDoctor(Guid doctorId);
    Task<IEnumerable<AppointmentDto>> GetAppointmentsForPatient(Guid patientId);
    Task<AppointmentDto> UpdateAppointmentStatus(Guid appointmentId, UpdateAppointmentDto updateDto);
    Task CancelAppointment(Guid appointmentId);
}