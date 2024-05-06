using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Api.Common;

public static class UserRolesCreation
{
    public static async Task CreateRoles(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
        string[] roleNames = { "Admin", "Patient", "Doctor" };

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                // Use the standard Name property provided by IdentityRole
                var roleResult = await roleManager.CreateAsync(new Role { Name = roleName });
                if (!roleResult.Succeeded)
                {
                    // Log errors or handle them
                    foreach (var error in roleResult.Errors)
                    {
                        Console.WriteLine($"Error creating role: {error.Description}");
                    }
                }
            }
        }
    }
}