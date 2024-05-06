namespace Api.Dtos;

public class PatientRequestInsertDto
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}