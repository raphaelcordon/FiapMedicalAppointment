namespace Domain.Entities;

public class Patient
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Cpf { get; set; }
    public string Address { get; set; }
    public string Zip { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    
    public void UpdateDetails(string firstName, string lastName, string cpf, string address, string zip)
    {
        FirstName = firstName;
        LastName = lastName;
        Cpf = cpf;
        Address = address;
        Zip = zip;
    }
}