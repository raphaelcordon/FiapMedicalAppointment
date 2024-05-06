namespace Api.Dtos;

public class PatientRequestDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Cpf { get; set; }
    public string Address { get; set; }
    public string Zip { get; set; }
    
    public UserDto UserDto { get; set; }
}