using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Impulse.Repository.Constants;
internal static class LocationConstants
{
    private static string DashboardFolder => "Impulse.Dashboard";

    private static string ConfigFile => "Dashboard.Config.db";

    internal static string LocalAppData => Environment.GetEnvironmentVariable("LocalAppData");

    internal static string DashboardDataLocation => Path.Combine(LocalAppData, DashboardFolder);

    internal static string DashboardConfigurationDatabase => Path.Combine(DashboardDataLocation, ConfigFile);
}
