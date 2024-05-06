using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Database;

public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        // Assuming 'Project' is the root and 'Api' is at the same level as 'Infrastructure'
        var projectRoot = Directory.GetCurrentDirectory(); // Gets 'Infrastructure'
        var apiPath = Path.Combine(Directory.GetParent(projectRoot).FullName, "Api");

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(apiPath)
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<DatabaseContext>();
        var connectionString = configuration.GetConnectionString("SqlServerConnectionString");
        builder.UseSqlServer(connectionString);

        return new DatabaseContext(builder.Options);
    }
}