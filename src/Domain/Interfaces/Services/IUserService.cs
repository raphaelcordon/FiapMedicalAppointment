using Domain.Dtos;

namespace Domain.Interfaces.Services;

public interface IUserService
{
    Task<IEnumerable<UserProfileResponseDto>> GetAllUsers();
    Task<UserProfileResponseDto> GetUserById(Guid id);
    Task<IEnumerable<UserProfileResponseDto>> GetUsersByRole(string roleName);
    Task<UserProfileResponseDto> UpdateUser(Guid id, UserUpdateDto dto);
    Task DeleteUser(Guid id);
}