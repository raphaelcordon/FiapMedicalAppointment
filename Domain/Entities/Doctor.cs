namespace Domain.Entities;

public class Patient
{
    public User User { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Cpf { get; set; }
    public string Address { get; set; }
    public string Zip { get; set; }
    public Appointment Appointment { get; set; }
    
    public void UpdateDetails(string firstName, string lastName, string cpf, string address, string zip, Appointment appointment)
    {
        FirstName = firstName;
        LastName = lastName;
        Cpf = cpf;
        Address = address;
        Zip = zip;
        Appointment = appointment;
    }
}