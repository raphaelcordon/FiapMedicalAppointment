using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Api.Common;

public class UserRolesCreation
{
    public static async Task CreateRoles(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>(); // Correct type here
        string[] roleNames = { "Admin", "Patient", "Doctor" };

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new Role { Name = roleName }); // Use custom Role constructor
            }
        }
    }
}