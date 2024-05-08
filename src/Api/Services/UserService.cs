using System.Data;
using Api.Common;
using Domain.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Api.Services;

public class UserService : IUserService
{
    private readonly IBaseRepository<UserProfile> _repository;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public UserService(IBaseRepository<UserProfile> repository, UserManager<User> userManager, IMapper mapper)
    {
        _repository = repository;
        _userManager = userManager;
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
    
    public async Task<IEnumerable<UserProfileResponseDto>> GetUsersByRole(string roleName)
    {
        var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
        if (!usersInRole.Any())
            throw new ResourceNotFoundException($"No users found with role {roleName}.");

        return _mapper.Map<IEnumerable<UserProfileResponseDto>>(usersInRole);
    }

    public async Task<UserProfileResponseDto> UpdateUser(Guid id, UserUpdateDto dto)
    {
        var user = await _repository.FindAsync(id);
        if (user == null)
            throw new ResourceNotFoundException($"No user found with the id: {id}");

        _mapper.Map(dto, user);
        _repository.Update(user);
        await _repository.SaveChangesAsync();

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
        _repository.Update(user);
        await _repository.SaveChangesAsync();
    }
}
