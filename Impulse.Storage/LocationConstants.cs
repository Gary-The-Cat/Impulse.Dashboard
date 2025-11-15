using System;
using System.IO;

namespace Impulse.Storage;

internal static class LocationConstants
{
    private static string DashboardFolder => "Impulse.Dashboard";

    private static string LogFile => "Dashboard.Logs.db";

    private static string PluginDataFile => "Dashboard.PluginData.db";

    internal static string LocalAppData => Environment.GetEnvironmentVariable("LocalAppData");

    internal static string DashboardDataLocation => Path.Combine(LocalAppData, DashboardFolder);

    internal static string DashboardLogDatabase => Path.Combine(DashboardDataLocation, LogFile);

    internal static string DashboardPluginDataDatabase => Path.Combine(DashboardDataLocation, PluginDataFile);
}
