using Impulse.Repository.Constants;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Impulse.Repository.Persistent;

public static class ConfigurationRepository
{
    /// <summary>
    /// Ensure that the database exists, and apply any unapplied migrations to ensure that it
    /// is up to date.
    /// </summary>
    /// <returns>A task to perform the initialization.</returns>
    public static void Initialize()
    {
        if (!Directory.Exists(LocationConstants.DashboardDataLocation))
        {
            Directory.CreateDirectory(LocationConstants.DashboardDataLocation);
        }

        var connectionString = GetConnectionStringForFile(LocationConstants.DashboardConfigurationDatabase);

        using (var context = new DashboardConfigurationDatabaseContext(connectionString))
        {
            context.Database.Migrate();
            context.SaveChanges();
        }
    }

    private static string GetConnectionStringForFile(string databaseFileLocation)
    {
        var dataSourceConnectionString = $"DataSource={databaseFileLocation}";
        return new SqliteConnectionStringBuilder(dataSourceConnectionString)
        {
            //Password = "MyPassword"
        }.ToString();
    }
}