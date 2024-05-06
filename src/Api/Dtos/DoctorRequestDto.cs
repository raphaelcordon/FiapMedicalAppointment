using Domain.Entities;

namespace Api.Dtos;

public class DoctorRequestDto
{
    public UserDto UserDto { get; set; }
    public string Role { get; set; }
}