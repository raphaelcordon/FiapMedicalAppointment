namespace Api.Dtos;

public class DoctorRequestInsertDto
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}