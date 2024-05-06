using System.ComponentModel.DataAnnotations;

namespace Api.Dtos;

public class RegisterDto
{
    [Required]
    public string Email { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string Password { get; set; }

    [Required]
    public string Role { get; set; } 
}