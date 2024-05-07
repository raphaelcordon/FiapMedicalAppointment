using Api.Dtos;

namespace Api.Services;

public interface IUserService
{
    Task<IEnumerable<UserProfileResponseDto>> GetAllUsers();
    Task<UserProfileResponseDto> GetUserById(Guid id);
    Task<UserProfileResponseDto> UpdateUser(Guid id, UserUpdateDto dto);
    Task DeleteUser(Guid id);
}