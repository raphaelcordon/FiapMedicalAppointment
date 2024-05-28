using Domain.Dtos;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IBaseRepository<Appointment> _appointmentRepository;
        private readonly IBaseRepository<AppointmentSpan> _spanRepository;
        private readonly IBaseRepository<MedicalSpecialty> _specialtyRepository;
        private readonly UserManager<UserProfile> _userManager;

        public AppointmentService(
            IBaseRepository<Appointment> appointmentRepository,
            IBaseRepository<AppointmentSpan> spanRepository,
            IBaseRepository<MedicalSpecialty> specialtyRepository,
            UserManager<UserProfile> userManager)
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
            if (span == null)
                throw new Exception("Invalid span.");

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
            var appointments = await _appointmentRepository.FindByCondition(a => a.DoctorId == doctorId && a.Status != "Cancelled")
                .Include(a => a.AppointmentSpan)
                .Include(a => a.Specialty)
                .ToListAsync();

            var doctor = await _userManager.FindByIdAsync(doctorId.ToString());

            return appointments.Select(a => new AppointmentDto
            {
                Id = a.Id,
                DoctorName = doctor.UserName,
                PatientName = a.Patient?.UserName,
                AppointmentTime = a.AppointmentDateTime,
                DurationMinutes = a.AppointmentSpan.Duration,
                Specialty = a.Specialty?.Specialty,
                Status = a.Status
            }).ToList();
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

        public async Task<AppointmentDto> UpdateAppointmentStatus(Guid appointmentId, UpdateAppointmentDto updateDto)
        {
            var appointment = await _appointmentRepository.FindAsync(appointmentId);
            if (appointment == null)
                throw new Exception("Appointment not found.");

            if (updateDto.NewAppointmentTime.HasValue)
                appointment.AppointmentDateTime = updateDto.NewAppointmentTime.Value;

            if (updateDto.NewSpan != Guid.Empty)
            {
                var span = await _spanRepository.FindAsync(updateDto.NewSpan);
                if (span == null)
                    throw new Exception("Invalid span.");
                appointment.AppointmentSpanId = updateDto.NewSpan;
            }

            if (!string.IsNullOrEmpty(updateDto.NewStatus))
                appointment.Status = updateDto.NewStatus;

            await _appointmentRepository.UpdateAsync(appointment);
            await _appointmentRepository.SaveChangesAsync();

            var doctor = await _userManager.FindByIdAsync(appointment.DoctorId.ToString());
            if (doctor == null)
                throw new Exception("Doctor not found.");

            var patient = await _userManager.FindByIdAsync(appointment.PatientId.ToString());
            if (patient == null)
                throw new Exception("Patient not found.");

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
            await _appointmentRepository.UpdateAsync(appointment);
            await _appointmentRepository.SaveChangesAsync();
        }
        
        public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByDateAsync(DateTime date)
        {
            var appointments = await _appointmentRepository.FindByCondition(a => a.AppointmentDateTime.Date == date)
                .Include(a => a.AppointmentSpan)
                .Include(a => a.Specialty)
                .ToListAsync();

            var doctorIds = appointments.Select(a => a.DoctorId).Distinct();
            var doctors = await _userManager.Users.Where(u => doctorIds.Contains(u.Id)).ToListAsync();
            var patientIds = appointments.Select(a => a.PatientId).Distinct();
            var patients = await _userManager.Users.Where(u => patientIds.Contains(u.Id)).ToListAsync();

            return appointments.Select(a =>
            {
                var doctor = doctors.FirstOrDefault(d => d.Id == a.DoctorId);
                var patient = patients.FirstOrDefault(p => p.Id == a.PatientId);
                return new AppointmentDto
                {
                    Id = a.Id,
                    DoctorName = doctor?.Email,
                    PatientName = patient?.Email,
                    PatientEmail = patient?.Email,
                    AppointmentTime = a.AppointmentDateTime,
                    DurationMinutes = a.AppointmentSpan.Duration,
                    Specialty = a.Specialty?.Specialty,
                    Status = a.Status
                };
            }).ToList();
        }
    }
}
