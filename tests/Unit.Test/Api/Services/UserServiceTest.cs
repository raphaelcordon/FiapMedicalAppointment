using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Common;
using Api.Services;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace Unit.Test.Api.Services
{
    public class UserServiceTest
    {
        private readonly Mock<IBaseRepository<UserProfile>> _userRepositoryMock;
        private readonly Mock<UserManager<UserProfile>> _userManagerMock;
        private readonly Mock<RoleManager<Role>> _roleManagerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UserService _userService;

        public UserServiceTest()
        {
            _userRepositoryMock = new Mock<IBaseRepository<UserProfile>>();
            _userManagerMock = new Mock<UserManager<UserProfile>>(Mock.Of<IUserStore<UserProfile>>(), null, null, null, null, null, null, null, null);
            _roleManagerMock = new Mock<RoleManager<Role>>(Mock.Of<IRoleStore<Role>>(), null, null, null, null);
            _mapperMock = new Mock<IMapper>();

            _userService = new UserService(
                _userRepositoryMock.Object,
                _userManagerMock.Object,
                _roleManagerMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnAllUsers()
        {
            // Arrange
            var users = new List<UserProfile>
            {
                new UserProfile { Id = Guid.NewGuid(), UserName = "User1" },
                new UserProfile { Id = Guid.NewGuid(), UserName = "User2" }
            };
            var userDtos = users.Select(u => new UserProfileResponseDto { Id = u.Id, Email = u.Email, Address = u.Address, PhoneNumber = u.PhoneNumber }).ToList();

            _userRepositoryMock.Setup(ur => ur.ListAsync()).ReturnsAsync(users);
            _userManagerMock.Setup(um => um.GetRolesAsync(It.IsAny<UserProfile>())).ReturnsAsync(new List<string>());
            _mapperMock.Setup(m => m.Map<UserProfileResponseDto>(It.IsAny<UserProfile>())).Returns((UserProfile source) => new UserProfileResponseDto { Id = source.Id, Email = source.Email, Address = source.Address, PhoneNumber = source.PhoneNumber });

            // Act
            var result = await _userService.GetAllUsers();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetUserById_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new UserProfile { Id = userId, UserName = "User1" };
            var userDto = new UserProfileResponseDto { Id = userId, Email = user.Email, Address = user.Address, PhoneNumber = user.PhoneNumber };

            _userRepositoryMock.Setup(ur => ur.FindAsync(userId)).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.GetRolesAsync(user)).ReturnsAsync(new List<string>());
            _mapperMock.Setup(m => m.Map<UserProfileResponseDto>(user)).Returns(userDto);

            // Act
            var result = await _userService.GetUserById(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
        }

        [Fact]
        public async Task GetUserById_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userRepositoryMock.Setup(ur => ur.FindAsync(userId)).ReturnsAsync((UserProfile)null);

            // Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(() => _userService.GetUserById(userId));
        }

        [Fact]
        public async Task GetUsersByRole_ShouldReturnUsers_WhenRoleExists()
        {
            // Arrange
            var roleName = "Admin";
            var role = new Role { Name = roleName };
            var users = new List<UserProfile>
            {
                new UserProfile { Id = Guid.NewGuid(), UserName = "User1" },
                new UserProfile { Id = Guid.NewGuid(), UserName = "User2" }
            };
            var userDtos = users.Select(u => new UserProfileResponseDto { Id = u.Id, Email = u.Email, Address = u.Address, PhoneNumber = u.PhoneNumber, Roles = new List<string> { roleName } }).ToList();

            _roleManagerMock.Setup(rm => rm.FindByNameAsync(roleName)).ReturnsAsync(role);
            _userRepositoryMock.Setup(ur => ur.ListAsync()).ReturnsAsync(users);
            _userManagerMock.Setup(um => um.IsInRoleAsync(It.IsAny<UserProfile>(), roleName)).ReturnsAsync(true);
            _mapperMock.Setup(m => m.Map<UserProfileResponseDto>(It.IsAny<UserProfile>())).Returns((UserProfile source) => new UserProfileResponseDto { Id = source.Id, Email = source.Email, Address = source.Address, PhoneNumber = source.PhoneNumber, Roles = new List<string> { roleName } });

            // Act
            var result = await _userService.GetUsersByRole(roleName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task UpdateUser_ShouldUpdateUser_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var updateDto = new UserUpdateDto { Address = "New Address", PhoneNumber = "1234567890" };
            var user = new UserProfile { Id = userId, UserName = "User1", Address = "Old Address", PhoneNumber = "0987654321" };
            var userDto = new UserProfileResponseDto { Id = userId, Address = "New Address", PhoneNumber = "1234567890" };

            _userRepositoryMock.Setup(ur => ur.FindAsync(userId)).ReturnsAsync(user);
            _userRepositoryMock.Setup(ur => ur.UpdateAsync(user)).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map(updateDto, user)).Returns(user);
            _mapperMock.Setup(m => m.Map<UserProfileResponseDto>(user)).Returns(userDto);
            _userManagerMock.Setup(um => um.GetRolesAsync(user)).ReturnsAsync(new List<string>());

            // Act
            var result = await _userService.UpdateUser(userId, updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Address", result.Address);
            Assert.Equal("1234567890", result.PhoneNumber);
        }

        [Fact]
        public async Task UpdateUser_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var updateDto = new UserUpdateDto { Address = "New Address", PhoneNumber = "1234567890" };

            _userRepositoryMock.Setup(ur => ur.FindAsync(userId)).ReturnsAsync((UserProfile)null);

            // Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(() => _userService.UpdateUser(userId, updateDto));
        }

        [Fact]
        public async Task DeleteUser_ShouldSetUserInactive_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new UserProfile { Id = userId, IsActive = true };

            _userRepositoryMock.Setup(ur => ur.FindAsync(userId)).ReturnsAsync(user);
            _userRepositoryMock.Setup(ur => ur.UpdateAsync(user)).Returns(Task.CompletedTask);

            // Act
            await _userService.DeleteUser(userId);

            // Assert
            Assert.False(user.IsActive);
            _userRepositoryMock.Verify(ur => ur.UpdateAsync(user), Times.Once);
        }

        [Fact]
        public async Task DeleteUser_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _userRepositoryMock.Setup(ur => ur.FindAsync(userId)).ReturnsAsync((UserProfile)null);

            // Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(() => _userService.DeleteUser(userId));
        }
    }
}
