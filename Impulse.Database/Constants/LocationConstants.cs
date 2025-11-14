using System;
using System.IO;

namespace Impulse.Repository.Constants;
internal static class LocationConstants
{
    private static string DashboardFolder => "Impulse.Dashboard";

    private static string LogFile => "Dashboard.Logs.db";

    internal static string LocalAppData => Environment.GetEnvironmentVariable("LocalAppData");

    internal static string DashboardDataLocation => Path.Combine(LocalAppData, DashboardFolder);

    internal static string DashboardLogDatabase => Path.Combine(DashboardDataLocation, LogFile);
}
