// <copyright file="DebugTabLoader.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Caliburn.Micro;
using Impulse.Dashboard.Debug.DemoScreens.AsyncBusyDemo;
using Impulse.Dashboard.Debug.ToolWindows;
using Impulse.Framework.Dashboard.Demonstrations;
using Impulse.SharedFramework.Ribbon;
using Impulse.SharedFramework.Services;
using Impulse.SharedFramework.Services.Logging;
using Ninject;
using System;

namespace Impulse.Dashboard.Debug;

public static class DebugTabLoader
{
    public static void LoadDebuggerTab(
        IKernel kernel,
        IRibbonService ribbonService)
    {
        ribbonService.AddTab(DebugRibbonIds.Tab_Debug, "Debug");

        ribbonService.AddGroup(DebugRibbonIds.Group_Test, "Tests");
        ribbonService.AddGroup(DebugRibbonIds.Group_Demos, "Functionality Demos");
        ribbonService.AddGroup(DebugRibbonIds.Group_Logging, "Logging");

        var logService = kernel.Get<ILogService>();

        var bottomToolWindowButton = new RibbonButtonViewModel()
        {
            Title = "Open Bottom Tool",
            Id = DebugRibbonIds.Button_OpenBottomToolWindow,
            EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Optimize.png",
            DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Optimize_GS.png",
            IsEnabled = true,
            Callback = () => OpenBottomToolWindow(kernel)
        };

        var exceptionDemo = new RibbonButtonViewModel()
        {
            Title = "Throw Exception",
            Id = DebugRibbonIds.Button_Exception,
            EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Coffin.png",
            DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Coffin_GS.png",
            IsEnabled = true,
            Callback = () => throw new System.Exception("You click the exception button")
        };

        var asyncBusyDemo = new RibbonButtonViewModel()
        {
            Title = "Async Busy",
            Id = DebugRibbonIds.Button_AsyncBusy,
            EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Spider.png",
            DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Spider_GS.png",
            IsEnabled = true,
            Callback = () => OpenAsyncBusyDemo(kernel)
        };

        var viewerDemo = new RibbonButtonViewModel()
        {
            Title = "Mono Demo",
            Id = DebugRibbonIds.Button_MonoDemo,
            EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Pumpkin.png",
            DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Pumpkin_GS.png",
            IsEnabled = true,
            Callback = () => OpenViewerDemo(kernel)
        };

        var infoLogButton = new RibbonButtonViewModel()
        {
            Title = "Log Info",
            Id = DebugRibbonIds.Button_LogInfo,
            EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Results.png",
            DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Results_GS.png",
            IsEnabled = true,
            Callback = () => _ = logService.LogInfo($"Debug info logged at {DateTime.Now:T}")
        };

        var warningLogButton = new RibbonButtonViewModel()
        {
            Title = "Log Warning",
            Id = DebugRibbonIds.Button_LogWarning,
            EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Results.png",
            DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Results_GS.png",
            IsEnabled = true,
            Callback = () => _ = logService.LogWarning($"Debug warning logged at {DateTime.Now:T}")
        };

        var errorLogButton = new RibbonButtonViewModel()
        {
            Title = "Log Error",
            Id = DebugRibbonIds.Button_LogError,
            EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Results.png",
            DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Results_GS.png",
            IsEnabled = true,
            Callback = () => _ = logService.LogError($"Debug error logged at {DateTime.Now:T}")
        };

        var exceptionLogButton = new RibbonButtonViewModel()
        {
            Title = "Log Exception",
            Id = DebugRibbonIds.Button_LogException,
            EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Results.png",
            DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Results_GS.png",
            IsEnabled = true,
            Callback = () => _ = logService.LogException($"Debug exception logged at {DateTime.Now:T}", new InvalidOperationException("Debug exception with stack trace"))
        };

        // Testing & Design
        ribbonService.AddButton(bottomToolWindowButton);
        ribbonService.AddButton(exceptionDemo);

        // Functionality Demo
        ribbonService.AddButton(asyncBusyDemo);

        // Viewer Demo
        ribbonService.AddButton(viewerDemo);

        // Logging Demo
        ribbonService.AddButton(infoLogButton);
        ribbonService.AddButton(warningLogButton);
        ribbonService.AddButton(errorLogButton);
        ribbonService.AddButton(exceptionLogButton);
    }

    private static void OpenAsyncBusyDemo(IKernel kernel)
    {
        var asyncBusyDemo = kernel.Get<AsyncBusyDemoViewModel>();
        var documentService = kernel.Get<IDocumentService>();

        documentService.OpenDocument(asyncBusyDemo);
    }

    private static BottomToolWindowViewModel? bottomToolWindow;

    private static void OpenBottomToolWindow(IKernel kernel)
    {
        var toolWindowService = kernel.Get<IToolWindowService>();

        bottomToolWindow ??= kernel.Get<BottomToolWindowViewModel>();

        toolWindowService.OpenBottomPaneToolWindow(bottomToolWindow);
    }

    private static void OpenViewerDemo(IKernel kernel)
    {
        var asyncBusyDemo = kernel.Get<ViewerDemoViewModel>();
        var documentService = kernel.Get<IDocumentService>();

        documentService.OpenDocument(asyncBusyDemo);
    }
}
