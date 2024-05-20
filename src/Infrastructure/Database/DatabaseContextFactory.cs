using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Database;

public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        var projectRoot = Directory.GetCurrentDirectory();
        var apiPath = Path.Combine(Directory.GetParent(projectRoot).FullName, "Api");

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(apiPath)
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        var builder = new DbContextOptionsBuilder<DatabaseContext>();

        // Check for environment variable first
        var connectionString = Environment.GetEnvironmentVariable("SQLSERVER_CONNECTIONSTRING") 
                               ?? configuration.GetConnectionString("SqlServerConnectionString");

        builder.UseSqlServer(connectionString);

        return new DatabaseContext(builder.Options);

    }
}