using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos;

public class UserUpdateDto
{
    public string Address { get; set; }
    
    public string PhoneNumber { get; set; }
}