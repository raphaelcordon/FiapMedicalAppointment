using System.ComponentModel.DataAnnotations;

namespace Api.Dtos;

public class UserUpdateDto
{
    public string Address { get; set; }
    
    [Phone]
    public string PhoneNumber { get; set; }
}