// <copyright file="DebugRibbonIds.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Impulse.Dashboard.Debug;

public class DebugRibbonIds
{
    public static string Dashboard => "Dashboard";

    public static string Tab_Debug => Concatenate(Dashboard, "Debug");

    public static string Tab_Config => Concatenate(Dashboard, "Config");

    // Debug Demos
    public static string Group_Demos => Concatenate(Tab_Debug, "Demos");

    public static string Group_Test => Concatenate(Tab_Debug, "Test");

    public static string Group_ProjectExplorer => Concatenate(Tab_Debug, "ProjectExplorer");

    public static string Button_Exception => Concatenate(Group_Test, "Exception");

    public static string Button_SeedProjectExplorer => Concatenate(Group_ProjectExplorer, "Seed");

    public static string Button_AsyncBusy => Concatenate(Group_Demos, "AsyncBusy");

    public static string Button_MonoDemo => Concatenate(Group_Demos, "MonoDemo");

    public static string Button_OpenBottomToolWindow => Concatenate(Group_Test, "BottomToolWindow");

    public static string Group_Logging => Concatenate(Tab_Debug, "Logging");

    public static string Button_LogInfo => Concatenate(Group_Logging, "Info");

    public static string Button_LogWarning => Concatenate(Group_Logging, "Warning");

    public static string Button_LogError => Concatenate(Group_Logging, "Error");

    public static string Button_LogException => Concatenate(Group_Logging, "Exception");

    public static string Concatenate(string a, string b) => $"{a}.{b}";
}
