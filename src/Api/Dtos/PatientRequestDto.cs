namespace Api.Dtos;

public class PatientRequestDto
{
    public string Cpf { get; set; }
    public string Address { get; set; }
    public string Zip { get; set; }
    public string Role { get; set; }
    
    public UserDto UserDto { get; set; }
}