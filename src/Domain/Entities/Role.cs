using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class Role : IdentityRole<Guid>

{
    public string RoleName { get; set; }
    public ICollection<User> Users { get; set; } = new List<User>();
}