using System.ComponentModel.DataAnnotations;

namespace Api.Dtos;

public class LoginRequestDto
{
    public string UserName { get; set; }
    
    [DataType(DataType.Password)]
    public string Password { get; set; }
}