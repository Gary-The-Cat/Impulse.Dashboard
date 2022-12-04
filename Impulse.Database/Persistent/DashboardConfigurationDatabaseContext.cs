using Microsoft.EntityFrameworkCore;

namespace Impulse.Repository.Persistent;

public class DashboardConfigurationDatabaseContext : DbContext, IDisposable
{
    private string connectionString;

    public ManagementDatabaseContext()
    {
        connectionString = "Data Source=:memory:";
    }

    public ManagementDatabaseContext(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public DbSet<DatabaseEmployee> Employees { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(connectionString);
    }
}