using Domain.Entities;

namespace Domain.Dtos;

public class UserProfileResponseDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
    public List<string> MedicalSpecialties { get; set; }
}