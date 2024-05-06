namespace Api.Dtos;

public class PatientRequestInsertDto
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Cpf { get; set; }
    public string Address { get; set; }
    public string Zip { get; set; }
}