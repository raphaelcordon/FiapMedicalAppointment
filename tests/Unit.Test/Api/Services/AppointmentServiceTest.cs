
using Api.Services;
using Domain.Dtos;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace Unit.Test.Api.Services
{
    public class AppointmentServiceTest
    {
        private readonly Mock<IBaseRepository<Appointment>> _appointmentRepositoryMock;
        private readonly Mock<IBaseRepository<AppointmentSpan>> _spanRepositoryMock;
        private readonly Mock<IBaseRepository<MedicalSpecialty>> _specialtyRepositoryMock;
        private readonly Mock<UserManager<UserProfile>> _userManagerMock;
        private readonly AppointmentService _appointmentService;

        public AppointmentServiceTest()
        {
            _appointmentRepositoryMock = new Mock<IBaseRepository<Appointment>>();
            _spanRepositoryMock = new Mock<IBaseRepository<AppointmentSpan>>();
            _specialtyRepositoryMock = new Mock<IBaseRepository<MedicalSpecialty>>();
            _userManagerMock = new Mock<UserManager<UserProfile>>(Mock.Of<IUserStore<UserProfile>>(), null, null, null, null, null, null, null, null);

            _appointmentService = new AppointmentService(
                _appointmentRepositoryMock.Object,
                _spanRepositoryMock.Object,
                _specialtyRepositoryMock.Object,
                _userManagerMock.Object);
        }

        [Fact]
        public async Task ScheduleAppointmentAsync_ShouldScheduleAppointment_WhenDataIsValid()
        {
            // Arrange
            var doctorId = Guid.NewGuid();
            var patientId = Guid.NewGuid();
            var spanId = Guid.NewGuid();
            var specialtyId = Guid.NewGuid();

            var scheduleDto = new ScheduleAppointmentDto
            {
                DoctorId = doctorId,
                PatientId = patientId,
                SpanId = spanId,
                SpecialtyId = specialtyId,
                AppointmentTime = DateTime.Now.AddDays(1)
            };

            var doctor = new UserProfile { Id = doctorId, UserName = "Doctor1" };
            var patient = new UserProfile { Id = patientId, UserName = "Patient1" };
            var span = new AppointmentSpan { Id = spanId, Duration = 30 };
            var specialty = new MedicalSpecialty { Id = specialtyId, Specialty = "Cardiology" };

            _userManagerMock.Setup(um => um.FindByIdAsync(doctorId.ToString())).ReturnsAsync(doctor);
            _userManagerMock.Setup(um => um.FindByIdAsync(patientId.ToString())).ReturnsAsync(patient);
            _userManagerMock.Setup(um => um.IsInRoleAsync(doctor, "Doctor")).ReturnsAsync(true);
            _userManagerMock.Setup(um => um.IsInRoleAsync(patient, "Patient")).ReturnsAsync(true);
            _spanRepositoryMock.Setup(sr => sr.FindAsync(spanId)).ReturnsAsync(span);
            _specialtyRepositoryMock.Setup(sr => sr.FindAsync(specialtyId)).ReturnsAsync(specialty);
            _appointmentRepositoryMock.Setup(ar => ar.AddAsync(It.IsAny<Appointment>())).Returns(Task.CompletedTask);
            _appointmentRepositoryMock.Setup(ar => ar.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _appointmentService.ScheduleAppointmentAsync(scheduleDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(doctor.UserName, result.DoctorName);
            Assert.Equal(patient.UserName, result.PatientName);
            Assert.Equal(scheduleDto.AppointmentTime, result.AppointmentTime);
            Assert.Equal(span.Duration, result.DurationMinutes);
            Assert.Equal(specialty.Specialty, result.Specialty);
            Assert.Equal("Scheduled", result.Status);
        }

        [Fact]
        public async Task ScheduleAppointmentAsync_ShouldThrowException_WhenDoctorIsInvalid()
        {
            // Arrange
            var scheduleDto = new ScheduleAppointmentDto
            {
                DoctorId = Guid.NewGuid(),
                PatientId = Guid.NewGuid(),
                SpanId = Guid.NewGuid(),
                SpecialtyId = Guid.NewGuid(),
                AppointmentTime = DateTime.Now.AddDays(1)
            };

            _userManagerMock.Setup(um => um.FindByIdAsync(scheduleDto.DoctorId.ToString())).ReturnsAsync((UserProfile)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _appointmentService.ScheduleAppointmentAsync(scheduleDto));
            Assert.Equal("Invalid doctor.", ex.Message);
        }

        [Fact]
        public async Task ScheduleAppointmentAsync_ShouldThrowException_WhenPatientIsInvalid()
        {
            // Arrange
            var doctorId = Guid.NewGuid();
            var scheduleDto = new ScheduleAppointmentDto
            {
                DoctorId = doctorId,
                PatientId = Guid.NewGuid(),
                SpanId = Guid.NewGuid(),
                SpecialtyId = Guid.NewGuid(),
                AppointmentTime = DateTime.Now.AddDays(1)
            };

            var doctor = new UserProfile { Id = doctorId, UserName = "Doctor1" };

            _userManagerMock.Setup(um => um.FindByIdAsync(doctorId.ToString())).ReturnsAsync(doctor);
            _userManagerMock.Setup(um => um.IsInRoleAsync(doctor, "Doctor")).ReturnsAsync(true);
            _userManagerMock.Setup(um => um.FindByIdAsync(scheduleDto.PatientId.ToString())).ReturnsAsync((UserProfile)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _appointmentService.ScheduleAppointmentAsync(scheduleDto));
            Assert.Equal("Invalid patient.", ex.Message);
        }

        [Fact]
        public async Task ScheduleAppointmentAsync_ShouldThrowException_WhenSpanIsInvalid()
        {
            // Arrange
            var doctorId = Guid.NewGuid();
            var patientId = Guid.NewGuid();
            var spanId = Guid.NewGuid();
            var specialtyId = Guid.NewGuid();

            var scheduleDto = new ScheduleAppointmentDto
            {
                DoctorId = doctorId,
                PatientId = patientId,
                SpanId = spanId,
                SpecialtyId = specialtyId,
                AppointmentTime = DateTime.Now.AddDays(1)
            };

            var doctor = new UserProfile { Id = doctorId, UserName = "Doctor1" };
            var patient = new UserProfile { Id = patientId, UserName = "Patient1" };

            _userManagerMock.Setup(um => um.FindByIdAsync(doctorId.ToString())).ReturnsAsync(doctor);
            _userManagerMock.Setup(um => um.FindByIdAsync(patientId.ToString())).ReturnsAsync(patient);
            _userManagerMock.Setup(um => um.IsInRoleAsync(doctor, "Doctor")).ReturnsAsync(true);
            _userManagerMock.Setup(um => um.IsInRoleAsync(patient, "Patient")).ReturnsAsync(true);
            _spanRepositoryMock.Setup(sr => sr.FindAsync(spanId)).ReturnsAsync((AppointmentSpan)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _appointmentService.ScheduleAppointmentAsync(scheduleDto));
            Assert.Equal("Invalid span.", ex.Message);
        }


        [Fact]
        public async Task ScheduleAppointmentAsync_ShouldThrowException_WhenSpecialtyIsInvalid()
        {
            // Arrange
            var doctorId = Guid.NewGuid();
            var patientId = Guid.NewGuid();
            var spanId = Guid.NewGuid();
            var specialtyId = Guid.NewGuid();

            var scheduleDto = new ScheduleAppointmentDto
            {
                DoctorId = doctorId,
                PatientId = patientId,
                SpanId = spanId,
                SpecialtyId = specialtyId,
                AppointmentTime = DateTime.Now.AddDays(1)
            };

            var doctor = new UserProfile { Id = doctorId, UserName = "Doctor1" };
            var patient = new UserProfile { Id = patientId, UserName = "Patient1" };
            var span = new AppointmentSpan { Id = spanId, Duration = 30 };

            _userManagerMock.Setup(um => um.FindByIdAsync(doctorId.ToString())).ReturnsAsync(doctor);
            _userManagerMock.Setup(um => um.FindByIdAsync(patientId.ToString())).ReturnsAsync(patient);
            _userManagerMock.Setup(um => um.IsInRoleAsync(doctor, "Doctor")).ReturnsAsync(true);
            _userManagerMock.Setup(um => um.IsInRoleAsync(patient, "Patient")).ReturnsAsync(true);
            _spanRepositoryMock.Setup(sr => sr.FindAsync(spanId)).ReturnsAsync(span);
            _specialtyRepositoryMock.Setup(sr => sr.FindAsync(specialtyId)).ReturnsAsync((MedicalSpecialty)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _appointmentService.ScheduleAppointmentAsync(scheduleDto));
            Assert.Equal("Invalid specialty.", ex.Message);
        }
    }
}
