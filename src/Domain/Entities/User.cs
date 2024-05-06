using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User : IdentityUser<Guid>
{
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public virtual Patient Patient { get; set; }
    public virtual Doctor Doctor { get; set; }
    
    public void UpdateDetails(string email)
    {
        Email = email;
    }
}