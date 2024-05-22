using Api.Common;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Unit.Test.Api.Services
{
    public class MedicalSpecialtyServiceTest
    {
        private readonly Mock<IBaseRepository<MedicalSpecialty>> _specialtyRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<UserManager<UserProfile>> _userManagerMock;
        private readonly MedicalSpecialtyService _medicalSpecialtyService;

        public MedicalSpecialtyServiceTest()
        {
            _specialtyRepositoryMock = new Mock<IBaseRepository<MedicalSpecialty>>();
            _mapperMock = new Mock<IMapper>();
            _userServiceMock = new Mock<IUserService>();
            _userManagerMock = new Mock<UserManager<UserProfile>>(Mock.Of<IUserStore<UserProfile>>(),
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null);

            _medicalSpecialtyService = new MedicalSpecialtyService(
                _specialtyRepositoryMock.Object,
                _mapperMock.Object,
                _userServiceMock.Object,
                _userManagerMock.Object);
        }

        [Fact]
        public async Task GetAllSpecialties_ShouldReturnAllSpecialties()
        {
            // Arrange
            var specialties = new List<MedicalSpecialty>
            {
                new MedicalSpecialty { Id = Guid.NewGuid(), Specialty = "Cardiology" },
                new MedicalSpecialty { Id = Guid.NewGuid(), Specialty = "Neurology" }
            };
            var specialtiesDto = specialties.Select(s => new MedicalSpecialtiesDtos.MedicalSpecialtyDto
                { Id = s.Id, Specialty = s.Specialty });

            _specialtyRepositoryMock.Setup(sr => sr.ListAsync()).ReturnsAsync(specialties);
            _mapperMock.Setup(m => m.Map<IEnumerable<MedicalSpecialtiesDtos.MedicalSpecialtyDto>>(specialties))
                .Returns(specialtiesDto);

            // Act
            var result = await _medicalSpecialtyService.GetAllSpecialties();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetSpecialtyById_ShouldReturnSpecialty_WhenSpecialtyExists()
        {
            // Arrange
            var specialtyId = Guid.NewGuid();
            var specialty = new MedicalSpecialty { Id = specialtyId, Specialty = "Cardiology" };
            var specialtyDto = new MedicalSpecialtiesDtos.MedicalSpecialtyDto
                { Id = specialtyId, Specialty = "Cardiology" };

            _specialtyRepositoryMock.Setup(sr => sr.FindAsync(specialtyId)).ReturnsAsync(specialty);
            _mapperMock.Setup(m => m.Map<MedicalSpecialtiesDtos.MedicalSpecialtyDto>(specialty)).Returns(specialtyDto);

            // Act
            var result = await _medicalSpecialtyService.GetSpecialtyById(specialtyId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(specialtyId, result.Id);
            Assert.Equal("Cardiology", result.Specialty);
        }

        [Fact]
        public async Task GetSpecialtyById_ShouldThrowException_WhenSpecialtyDoesNotExist()
        {
            // Arrange
            var specialtyId = Guid.NewGuid();
            _specialtyRepositoryMock.Setup(sr => sr.FindAsync(specialtyId)).ReturnsAsync((MedicalSpecialty)null);

            // Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(() =>
                _medicalSpecialtyService.GetSpecialtyById(specialtyId));
        }

        [Fact]
        public async Task AddSpecialty_ShouldAddSpecialty()
        {
            // Arrange
            var createDto = new MedicalSpecialtiesDtos.CreateMedicalSpecialtyDto { Specialty = "Cardiology" };
            var specialty = new MedicalSpecialty { Id = Guid.NewGuid(), Specialty = "Cardiology" };
            var specialtyDto = new MedicalSpecialtiesDtos.MedicalSpecialtyDto
                { Id = specialty.Id, Specialty = "Cardiology" };

            _mapperMock.Setup(m => m.Map<MedicalSpecialty>(createDto)).Returns(specialty);
            _specialtyRepositoryMock.Setup(sr => sr.AddAsync(specialty)).Returns(Task.CompletedTask);
            _specialtyRepositoryMock.Setup(sr => sr.SaveChangesAsync()).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<MedicalSpecialtiesDtos.MedicalSpecialtyDto>(specialty)).Returns(specialtyDto);

            // Act
            var result = await _medicalSpecialtyService.AddSpecialty(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Cardiology", result.Specialty);
        }

        [Fact]
        public async Task UpdateSpecialty_ShouldUpdateSpecialty_WhenSpecialtyExists()
        {
            // Arrange
            var specialtyId = Guid.NewGuid();
            var updateDto = new MedicalSpecialtiesDtos.UpdateMedicalSpecialtyDto { Specialty = "Updated Specialty" };
            var specialty = new MedicalSpecialty { Id = specialtyId, Specialty = "Original Specialty" };
            var specialtyDto = new MedicalSpecialtiesDtos.MedicalSpecialtyDto
                { Id = specialtyId, Specialty = "Updated Specialty" };

            _specialtyRepositoryMock.Setup(sr => sr.FindAsync(specialtyId)).ReturnsAsync(specialty);
            _specialtyRepositoryMock.Setup(sr => sr.UpdateAsync(specialty)).Returns(Task.CompletedTask);
            _specialtyRepositoryMock.Setup(sr => sr.SaveChangesAsync()).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<MedicalSpecialtiesDtos.MedicalSpecialtyDto>(specialty)).Returns(specialtyDto);

            // Act
            var result = await _medicalSpecialtyService.UpdateSpecialty(specialtyId, updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(specialtyId, result.Id);
            Assert.Equal("Updated Specialty", result.Specialty);
        }

        [Fact]
        public async Task UpdateSpecialty_ShouldThrowException_WhenSpecialtyDoesNotExist()
        {
            // Arrange
            var specialtyId = Guid.NewGuid();
            var updateDto = new MedicalSpecialtiesDtos.UpdateMedicalSpecialtyDto { Specialty = "Updated Specialty" };

            _specialtyRepositoryMock.Setup(sr => sr.FindAsync(specialtyId)).ReturnsAsync((MedicalSpecialty)null);

            // Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(() =>
                _medicalSpecialtyService.UpdateSpecialty(specialtyId, updateDto));
        }

        [Fact]
        public async Task DeleteSpecialty_ShouldDeleteSpecialty_WhenSpecialtyExists()
        {
            // Arrange
            var specialtyId = Guid.NewGuid();

            _specialtyRepositoryMock.Setup(sr => sr.DeleteAsync(specialtyId)).Returns(Task.CompletedTask);
            _specialtyRepositoryMock.Setup(sr => sr.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _medicalSpecialtyService.DeleteSpecialty(specialtyId);

            // Assert
            _specialtyRepositoryMock.Verify(sr => sr.DeleteAsync(specialtyId), Times.Once);
            _specialtyRepositoryMock.Verify(sr => sr.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task AddSpecialtyToDoctor_ShouldAddSpecialty_WhenDoctorAndSpecialtyExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var specialtyId = Guid.NewGuid();
            var user = new UserProfile
                { Id = userId, UserName = "Doctor1", MedicalSpecialties = new List<MedicalSpecialty>() };
            var specialty = new MedicalSpecialty { Id = specialtyId, Specialty = "Cardiology" };

            _userManagerMock.Setup(um => um.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.IsInRoleAsync(user, "Doctor")).ReturnsAsync(true);
            _specialtyRepositoryMock.Setup(sr => sr.FindAsync(specialtyId)).ReturnsAsync(specialty);
            _userManagerMock.Setup(um => um.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            // Act
            await _medicalSpecialtyService.AddSpecialtyToDoctor(userId, specialtyId);

            // Assert
            Assert.Contains(specialty, user.MedicalSpecialties);
        }
    }
}