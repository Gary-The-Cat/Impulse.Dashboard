namespace Impulse.Storage;

using System;
using System.IO;
using Microsoft.Data.Sqlite;

public static class DashboardStorage
{
    public static string GetLogConnectionString()
    {
        EnsureDataDirectory();
        return BuildConnectionString(LocationConstants.DashboardLogDatabase);
    }

    public static string GetPluginDataConnectionString()
    {
        EnsureDataDirectory();
        return BuildConnectionString(LocationConstants.DashboardPluginDataDatabase);
    }

    private static void EnsureDataDirectory()
    {
        if (string.IsNullOrWhiteSpace(LocationConstants.LocalAppData))
        {
            throw new InvalidOperationException("LocalAppData environment variable was not set.");
        }

        if (!Directory.Exists(LocationConstants.DashboardDataLocation))
        {
            Directory.CreateDirectory(LocationConstants.DashboardDataLocation);
        }
    }

    private static string BuildConnectionString(string databaseFileLocation)
    {
        var dataSourceConnectionString = $"DataSource={databaseFileLocation}";
        return new SqliteConnectionStringBuilder(dataSourceConnectionString)
        {
            ForeignKeys = true,
        }.ToString();
    }
}
