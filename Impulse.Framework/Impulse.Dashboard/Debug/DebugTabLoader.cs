// <copyright file="DebugTabLoader.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Caliburn.Micro;
using Impulse.Dashboard.Debug.DemoScreens.AsyncBusyDemo;
using Impulse.Framework.Dashboard.Demonstrations;
using Impulse.SharedFramework.Ribbon;
using Impulse.SharedFramework.Services;
using Ninject;

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

        // Testing & Design
        ribbonService.AddButton(exceptionDemo);

        // Functionality Demo
        ribbonService.AddButton(asyncBusyDemo);

        // Viewer Demo
        ribbonService.AddButton(viewerDemo);
    }

    private static void OpenAsyncBusyDemo(IKernel kernel)
    {
        var asyncBusyDemo = kernel.Get<AsyncBusyDemoViewModel>();
        var documentService = kernel.Get<IDocumentService>();

        documentService.OpenDocument(asyncBusyDemo);
    }

    private static void OpenViewerDemo(IKernel kernel)
    {
        var asyncBusyDemo = kernel.Get<ViewerDemoViewModel>();
        var documentService = kernel.Get<IDocumentService>();

        documentService.OpenDocument(asyncBusyDemo);
    }
}
