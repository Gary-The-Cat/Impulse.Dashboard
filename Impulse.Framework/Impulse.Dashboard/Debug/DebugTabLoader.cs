// <copyright file="DebugTabLoader.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Caliburn.Micro;
using Impulse.Dashboard.Debug.DemoScreens.AsyncBusyDemo;
using Impulse.Dashboard.Debug.DemoScreens.LlmChat;
using Impulse.Dashboard.Debug.ToolWindows;
using Impulse.Framework.Dashboard.Demonstrations;
using Impulse.SharedFramework.Ribbon;
using Impulse.SharedFramework.Services;
using Impulse.SharedFramework.Services.Layout;
using Impulse.Logging.Contracts;
using Ninject;
using System;
using System.Threading.Tasks;

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
        ribbonService.AddGroup(DebugRibbonIds.Group_ProjectExplorer, "Project Explorer");
        ribbonService.AddGroup(DebugRibbonIds.Group_Logging, "Logging");
        ribbonService.AddGroup(DebugRibbonIds.Group_Llm, "LLM");

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

        var seedProjectExplorerButton = new RibbonButtonViewModel()
        {
            Title = "Seed Explorer",
            Id = DebugRibbonIds.Button_SeedProjectExplorer,
            EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Project.png",
            DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Project_GS.png",
            IsEnabled = true,
            Callback = async () => await SeedProjectExplorerAsync(kernel)
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
            Callback = () =>
            {
                var exception = CreateDebugException();
                _ = logService.LogException($"Debug exception logged at {DateTime.Now:T}", exception);
            },
        };

        var llmChatButton = new RibbonButtonViewModel()
        {
            Title = "LLM Chat",
            Id = DebugRibbonIds.Button_LlmChat,
            EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Configuration.png",
            DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Configuration_GS.png",
            IsEnabled = true,
            Callback = () => OpenLlmChat(kernel)
        };

        // Testing & Design
        ribbonService.AddButton(bottomToolWindowButton);
        ribbonService.AddButton(exceptionDemo);

        // Functionality Demo
        ribbonService.AddButton(asyncBusyDemo);

        // Project Explorer Utilities
        ribbonService.AddButton(seedProjectExplorerButton);

        // Viewer Demo
        ribbonService.AddButton(viewerDemo);

        // Logging Demo
        ribbonService.AddButton(infoLogButton);
        ribbonService.AddButton(warningLogButton);
        ribbonService.AddButton(errorLogButton);
        ribbonService.AddButton(exceptionLogButton);

        // LLM Tools
        ribbonService.AddButton(llmChatButton);
    }

    private static void OpenAsyncBusyDemo(IKernel kernel)
    {
        var asyncBusyDemo = kernel.Get<AsyncBusyDemoViewModel>();
        var documentService = kernel.Get<IDocumentService>();

        documentService.OpenDocument(asyncBusyDemo);
    }

    private static BottomToolWindowViewModel? bottomToolWindow;

    private static LlmChatViewModel? llmChatViewModel;

    private static void OpenBottomToolWindow(IKernel kernel)
    {
        var toolWindowService = kernel.Get<IToolWindowService>();

        bottomToolWindow ??= kernel.Get<BottomToolWindowViewModel>();

        toolWindowService.OpenBottomPaneToolWindow(bottomToolWindow);
    }

    private static Exception CreateDebugException()
    {
        try
        {
            ThrowDebugException();
            throw new InvalidOperationException("Unreachable");
        }
        catch (Exception exception)
        {
            return exception;
        }
    }

    private static void ThrowDebugException() =>
        throw new InvalidOperationException("Debug exception with stack trace");

    private static void OpenViewerDemo(IKernel kernel)
    {
        var asyncBusyDemo = kernel.Get<ViewerDemoViewModel>();
        var documentService = kernel.Get<IDocumentService>();

        documentService.OpenDocument(asyncBusyDemo);
    }

    private static void OpenLlmChat(IKernel kernel)
    {
        var documentService = kernel.Get<IDocumentService>();
        llmChatViewModel ??= kernel.Get<LlmChatViewModel>();
        documentService.OpenDocument(llmChatViewModel);
    }

    private static async Task SeedProjectExplorerAsync(IKernel kernel)
    {
        var explorer = kernel.Get<IProjectExplorerService>();
        explorer.ClearAllItems();

        var root = new ProjectExplorerFolder
        {
            DisplayName = "Sample Solution",
            Icon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Project.png",
            IsExpanded = true,
            IsEditing = false
        };
        await explorer.AddItemAsync(root);

        var srcFolder = new ProjectExplorerFolder
        {
            DisplayName = "src",
            ParentId = root.Id,
            IsExpanded = true,
            IsEditing = false
        };
        await explorer.AddItemAsync(srcFolder);

        var dashboardsFolder = new ProjectExplorerFolder
        {
            DisplayName = "Dashboard",
            ParentId = srcFolder.Id,
            IsExpanded = true,
            IsEditing = false
        };
        await explorer.AddItemAsync(dashboardsFolder);

        var servicesFolder = new ProjectExplorerFolder
        {
            DisplayName = "Services",
            ParentId = srcFolder.Id,
            IsExpanded = true,
            IsEditing = false
        };
        await explorer.AddItemAsync(servicesFolder);

        var loggingFolder = new ProjectExplorerFolder
        {
            DisplayName = "Logging",
            ParentId = servicesFolder.Id,
            IsExpanded = true,
            IsEditing = false
        };
        await explorer.AddItemAsync(loggingFolder);

        await explorer.AddItemAsync(CreateLeaf("ShellView.xaml", dashboardsFolder.Id));
        await explorer.AddItemAsync(CreateLeaf("ShellViewModel.cs", dashboardsFolder.Id));
        await explorer.AddItemAsync(CreateLeaf("DialogService.cs", servicesFolder.Id));
        await explorer.AddItemAsync(CreateLeaf("LogService.cs", loggingFolder.Id));
        await explorer.AddItemAsync(CreateLeaf("LogWindowViewModel.cs", loggingFolder.Id));

        var testsFolder = new ProjectExplorerFolder
        {
            DisplayName = "tests",
            ParentId = root.Id,
            IsExpanded = true,
            IsEditing = false
        };
        await explorer.AddItemAsync(testsFolder);

        await explorer.AddItemAsync(CreateLeaf("ProjectExplorerViewModelTests.cs", testsFolder.Id));
        await explorer.AddItemAsync(CreateLeaf("README.md", root.Id, icon: "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Open.png"));
    }

    private static ProjectExplorerItemBase CreateLeaf(string name, Guid parentId, string icon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Open.png")
    {
        return new ProjectExplorerItemBase
        {
            DisplayName = name,
            ParentId = parentId,
            Icon = icon,
            IsEditable = true,
            IsExpanded = false,
            IsEditing = false
        };
    }
}
