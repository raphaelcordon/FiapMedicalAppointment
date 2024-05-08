using Domain.Dtos;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IBaseRepository<Appointment> _appointmentRepository;
    private readonly IBaseRepository<AppointmentSpan> _spanRepository;
    private readonly IBaseRepository<MedicalSpecialty> _specialtyRepository;
    private readonly UserManager<User> _userManager;

    public AppointmentService(
        IBaseRepository<Appointment> appointmentRepository,
        IBaseRepository<AppointmentSpan> spanRepository,
        IBaseRepository<MedicalSpecialty> specialtyRepository,
        UserManager<User> userManager)
    {
        _appointmentRepository = appointmentRepository;
        _spanRepository = spanRepository;
        _specialtyRepository = specialtyRepository;
        _userManager = userManager;
    }

    public async Task<AppointmentDto> ScheduleAppointmentAsync(ScheduleAppointmentDto scheduleDto)
    {
        var doctor = await _userManager.FindByIdAsync(scheduleDto.DoctorId.ToString());
        if (doctor == null || !(await _userManager.IsInRoleAsync(doctor, "Doctor")))
            throw new Exception("Invalid doctor.");

        var patient = await _userManager.FindByIdAsync(scheduleDto.PatientId.ToString());
        if (patient == null || !(await _userManager.IsInRoleAsync(patient, "Patient")))
            throw new Exception("Invalid patient.");

        var span = await _spanRepository.FindAsync(scheduleDto.SpanId);
        var specialty = await _specialtyRepository.FindAsync(scheduleDto.SpecialtyId);
        if (specialty == null)
            throw new Exception("Invalid specialty.");

        var appointment = new Appointment
        {
            DoctorId = scheduleDto.DoctorId,
            PatientId = scheduleDto.PatientId,
            AppointmentDateTime = scheduleDto.AppointmentTime,
            AppointmentSpanId = scheduleDto.SpanId,
            SpecialtyId = scheduleDto.SpecialtyId,
            Status = "Scheduled"
        };

        await _appointmentRepository.AddAsync(appointment);
        await _appointmentRepository.SaveChangesAsync();

        return new AppointmentDto
        {
            Id = appointment.Id,
            DoctorName = doctor.UserName,
            PatientName = patient.UserName,
            AppointmentTime = appointment.AppointmentDateTime,
            DurationMinutes = span.Duration,
            Specialty = specialty.Specialty,
            Status = appointment.Status
        };
    }

    public async Task<IEnumerable<AppointmentDto>> GetAppointmentsForDoctor(Guid doctorId)
    {
        try
        {
            var query = _appointmentRepository.FindByCondition(a => a.DoctorId == doctorId && a.Status != "Cancelled");
            var appointments = await query
                .Include(a => a.AppointmentSpan)
                .Include(a => a.Specialty)
                .ToListAsync();

            if (!appointments.Any())
            {
                // Log the absence of appointments or handle accordingly
                return Enumerable.Empty<AppointmentDto>();
            }

            return appointments.Select(a => new AppointmentDto
            {
                Id = a.Id,
                DoctorName = a.Doctor?.UserName ?? "Unknown Doctor",
                PatientName = a.Patient?.UserName ?? "Unknown Patient",
                AppointmentTime = a.AppointmentDateTime,
                DurationMinutes = a.AppointmentSpan.Duration,
                Specialty = a.Specialty?.Specialty ?? "Unknown Specialty",
                Status = a.Status
            }).ToList();
        }
        catch (Exception ex)
        {
            // Log the exception details here to help with debugging
            throw new Exception("Failed to fetch appointments for doctor. Error: " + ex.Message);
        }
    }

    public async Task<IEnumerable<AppointmentDto>> GetAppointmentsForPatient(Guid patientId)
    {
        var appointments = await _appointmentRepository.FindByCondition(a => a.PatientId == patientId && a.Status != "Cancelled")
            .Include(a => a.AppointmentSpan)
            .Include(a => a.Specialty)
            .ToListAsync();

        var doctorIds = appointments.Select(a => a.DoctorId).Distinct();
        var doctors = await _userManager.Users.Where(u => doctorIds.Contains(u.Id)).ToListAsync();
        var patient = await _userManager.FindByIdAsync(patientId.ToString());

        return appointments.Select(a =>
        {
            var doctor = doctors.FirstOrDefault(d => d.Id == a.DoctorId);
            return new AppointmentDto
            {
                Id = a.Id,
                DoctorName = doctor?.UserName,
                PatientName = patient?.UserName,
                AppointmentTime = a.AppointmentDateTime,
                DurationMinutes = a.AppointmentSpan.Duration,
                Specialty = a.Specialty?.Specialty,
                Status = a.Status
            };
        }).ToList();
    }

    public async Task<AppointmentDto> UpdateAppointmentStatus(Guid appointmentId, string status)
    {
        var appointment = await _appointmentRepository.FindAsync(appointmentId);
        if (appointment == null)
            throw new Exception("Appointment not found.");

        appointment.Status = status;
        _appointmentRepository.Update(appointment);
        await _appointmentRepository.SaveChangesAsync();

        var doctor = await _userManager.FindByIdAsync(appointment.DoctorId.ToString());
        var patient = await _userManager.FindByIdAsync(appointment.PatientId.ToString());

        return new AppointmentDto
        {
            Id = appointment.Id,
            DoctorName = doctor?.UserName,
            PatientName = patient?.UserName,
            AppointmentTime = appointment.AppointmentDateTime,
            DurationMinutes = appointment.AppointmentSpan.Duration,
            Specialty = appointment.Specialty?.Specialty,
            Status = appointment.Status
        };
    }

    public async Task CancelAppointment(Guid appointmentId)
    {
        var appointment = await _appointmentRepository.FindAsync(appointmentId);
        if (appointment == null)
            throw new Exception("Appointment not found.");

        appointment.Status = "Cancelled";
        _appointmentRepository.Update(appointment);
        await _appointmentRepository.SaveChangesAsync();
    }
}
