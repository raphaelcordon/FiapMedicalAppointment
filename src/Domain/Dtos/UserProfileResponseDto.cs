using Domain.Entities;

namespace Api.Dtos;

public class UserProfileResponseDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
}