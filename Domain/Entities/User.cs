namespace Domain.Entities;

public class Users : BaseEntity
{
    public string Email { get; set; }
    public string Password { get; set; }
    public byte[] Salt { get; set; }
    public string Role { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public void UpdateDetails(string email, string password, string role)
    {
        Email = email;
        Password = password;
        Role = role;
    }
}