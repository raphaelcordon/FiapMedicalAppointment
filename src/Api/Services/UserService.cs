using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Common;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<UserProfile> _repository;
        private readonly UserManager<UserProfile> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public UserService(IBaseRepository<UserProfile> repository, UserManager<UserProfile> userManager, RoleManager<Role> roleManager, IMapper mapper)
        {
            _repository = repository;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserProfileResponseDto>> GetAllUsers()
        {
            var users = await _repository.ListAsync();
            var userDtos = new List<UserProfileResponseDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userDto = _mapper.Map<UserProfileResponseDto>(user);
                userDto.Roles = roles.ToList();
                userDtos.Add(userDto);
            }
            return userDtos;
        }

        public async Task<UserProfileResponseDto> GetUserById(Guid id)
        {
            var user = await _repository.FindAsync(id);
            if (user == null)
                throw new ResourceNotFoundException($"No user found with the id: {id}");

            var roles = await _userManager.GetRolesAsync(user);
            var userDto = _mapper.Map<UserProfileResponseDto>(user);
            userDto.Roles = roles.ToList();
            return userDto;
        }

        public async Task<List<UserProfileResponseDto>> GetUsersByRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return new List<UserProfileResponseDto>();
            }

            var userRoles = await _repository.ListAsync();

            var usersInRole = userRoles.Where(u => _userManager.IsInRoleAsync(u, roleName).Result).ToList();
            var users = usersInRole.Select(user => new UserProfileResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                Roles = new List<string> { roleName },
                MedicalSpecialties = user.MedicalSpecialties.Select(ms => ms.Specialty).ToList()
            }).ToList();

            return users;
        }

        public async Task<UserProfileResponseDto> UpdateUser(Guid id, UserUpdateDto dto)
        {
            var user = await _repository.FindAsync(id);
            if (user == null)
                throw new ResourceNotFoundException($"No user found with the id: {id}");

            _mapper.Map(dto, user);
            await _repository.UpdateAsync(user); // Use UpdateAsync method
            var roles = await _userManager.GetRolesAsync(user);
            var userDto = _mapper.Map<UserProfileResponseDto>(user);
            userDto.Roles = roles.ToList();
            return userDto;
        }

        public async Task DeleteUser(Guid id)
        {
            var user = await _repository.FindAsync(id);
            if (user == null)
                throw new ResourceNotFoundException($"No user found with the id: {id}");

            user.IsActive = false;
            await _repository.UpdateAsync(user); // Use UpdateAsync method
        }
    }
}
