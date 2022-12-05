using System;

namespace Impulse.Shared;

public static class GlobalConstants
{
    public static string LocalAppData => Environment.GetEnvironmentVariable("LocalAppData");
}