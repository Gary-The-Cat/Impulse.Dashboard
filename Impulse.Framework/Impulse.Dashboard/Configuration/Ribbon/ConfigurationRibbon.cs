using Impulse.Dashboard.Debug;
using Impulse.SharedFramework.Services;

namespace Impulse.Framework.Dashboard.Configuration.Ribbon;

internal static class ConfigurationRibbon
{
    internal static void LoadConfigTab(IRibbonService ribbonService)
    {
        ribbonService.AddTab(DebugRibbonIds.Tab_Config, "Config");
    }
}
