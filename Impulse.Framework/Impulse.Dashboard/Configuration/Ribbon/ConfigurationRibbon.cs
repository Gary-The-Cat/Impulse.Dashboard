using Caliburn.Micro;
using Impulse.Dashboard.Debug;
using Impulse.Framework.Dashboard.Configuration.Screens;
using Impulse.SharedFramework.Ribbon;
using Impulse.SharedFramework.Services;
using Ninject;

namespace Impulse.Framework.Dashboard.Configuration.Ribbon;

internal static class ConfigurationRibbon
{
    internal static void LoadConfigTab(IKernel kernel, IRibbonService ribbonService)
    {
        ribbonService.AddTab(DebugRibbonIds.Tab_Config, "Config");

        ribbonService.AddGroup(DebugRibbonIds.Group_Logs, "Logs");

        var exceptionDemo = new RibbonButtonViewModel()
        {
            Title = "Log Settings",
            Id = DebugRibbonIds.Button_LogSettings,
            EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Moon.png",
            DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Moon_GS.png",
            IsEnabled = true,
            Callback = () => OpenLogSettings(kernel),
        };

        ribbonService.AddButton(exceptionDemo);
    }

    private static void OpenLogSettings(IKernel kernel)
    {
        var windowManager = kernel.Get<IWindowManager>();
        var window = kernel.Get<LogSettingsViewModel>();

        windowManager.ShowDialogAsync(window);
    }
}
