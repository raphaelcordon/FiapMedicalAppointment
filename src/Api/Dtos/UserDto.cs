using Domain.Entities;

namespace Api.Dtos;

public class UserDto
{
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}