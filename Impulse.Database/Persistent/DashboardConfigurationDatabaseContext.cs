using Impulse.Repository.DataStructures;
using Microsoft.EntityFrameworkCore;

namespace Impulse.Repository.Persistent;

public class DashboardConfigurationDatabaseContext : DbContext, IDisposable
{
    private string connectionString;

    public DashboardConfigurationDatabaseContext(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public DbSet<DashboardConfigurationModel> Configuration { get; init; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(connectionString);
    }
}