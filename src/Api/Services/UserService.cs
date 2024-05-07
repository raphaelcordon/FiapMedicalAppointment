using System.Data;
using Api.Common;
using Api.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

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
        return _mapper.Map<IEnumerable<UserProfileResponseDto>>(users);
    }

    public async Task<UserProfileResponseDto> GetUserById(Guid id)
    {
        var user = await _repository.FindAsync(id);
        if (user == null)
            throw new ResourceNotFoundException($"No user found with the id: {id}");

        return _mapper.Map<UserProfileResponseDto>(user);
    }

    public async Task<UserProfileResponseDto> UpdateUser(Guid id, UserUpdateDto dto)
    {
        var user = await _repository.FindAsync(id);
        if (user == null)
            throw new ResourceNotFoundException($"No user found with the id: {id}");

        _mapper.Map(dto, user);
        _repository.Update(user);
        await _repository.SaveChangesAsync();

        return _mapper.Map<UserProfileResponseDto>(user);
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