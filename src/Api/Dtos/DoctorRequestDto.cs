namespace Api.Dtos;

public class DoctorRequestDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public UserDto UserDto { get; set; }
}