// <copyright file="DebugTabLoader.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using Impulse.Dashboard.Debug.DemoScreens.AsyncBusyDemo;
using Impulse.Dashboard.Debug.DemoScreens.BindingDemo;
using Impulse.Dashboard.Debug.DemoScreens.BindingDemo2;
using Impulse.Dashboard.Debug.DemoScreens.BindingDemo3;
using Impulse.Dashboard.Debug.DemoScreens.DirectionsDemo;
using Impulse.Dashboard.Debug.DemoScreens.TemplatePractice;
using Impulse.Dashboard.Debug.DemoScreens.UserControlDemo;
using Impulse.Shared.Services;
using Impulse.SharedFramework.Ribbon;
using Impulse.SharedFramework.Services;
using Impulse.SharedFramework.ToastNotifications;
using Ninject;

namespace Impulse.Dashboard.Debug;

public static class DebugTabLoader
{
    public static void LoadDebuggerTab(
        IKernel kernel,
        IRibbonService ribbonService)
    {
        ribbonService.AddTab(DebugRibbonIds.DebugTab, "Debug");

        ribbonService.AddGroup(DebugRibbonIds.TaylorGroup, "Taylors Demos");

        ribbonService.AddGroup(DebugRibbonIds.TestGroup, "Tests");

        ribbonService.AddGroup(DebugRibbonIds.DemosGroup, "Functionality Demos");

        var bindingDemo = new RibbonButtonViewModel()
        {
            Title = "Binding Demo",
            Id = DebugRibbonIds.BindingDemo,
            EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Pumpkin.png",
            DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Pumpkin_GS.png",
            IsEnabled = true,
            Callback = () => OpenBindingDemo(kernel)
        };

        var bindingDemo2 = new RibbonButtonViewModel()
        {
            Title = "Binding Demo 2",
            Id = DebugRibbonIds.BindingDemo2,
            EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Couldren.png",
            DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Couldren_GS.png",
            IsEnabled = true,
            Callback = () => OpenBindingDemo2(kernel)
        };

        var bindingDemo3 = new RibbonButtonViewModel()
        {
            Title = "Binding Demo 3",
            Id = DebugRibbonIds.BindingDemo3,
            EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Ghost.png",
            DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Ghost_GS.png",
            IsEnabled = true,
            Callback = () => OpenBindingDemo3(kernel)
        };

        var userControlDemo = new RibbonButtonViewModel()
        {
            Title = "User Control Demo",
            Id = DebugRibbonIds.UserControlDemo,
            EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Monster.png",
            DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Monster_GS.png",
            IsEnabled = true,
            Callback = () => OpenUserControlDemo(kernel)
        };

        var templatePractice = new RibbonButtonViewModel()
        {
            Title = "Template Practice",
            Id = DebugRibbonIds.TemplatePractice,
            EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Moon.png",
            DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Moon_GS.png",
            IsEnabled = true,
            Callback = () => OpenTemplatePractice(kernel)
        };

        var exceptionDemo = new RibbonButtonViewModel()
        {
            Title = "Throw Exception",
            Id = DebugRibbonIds.ExceptionDemo,
            EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Coffin.png",
            DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Coffin_GS.png",
            IsEnabled = true,
            Callback = () => throw new System.Exception("You click the exception button")
        };

        var asyncBusyDemo = new RibbonButtonViewModel()
        {
            Title = "Async Busy",
            Id = DebugRibbonIds.AsyncBusyDemo,
            EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Spider.png",
            DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Spider_GS.png",
            IsEnabled = true,
            Callback = () => OpenAsyncBusyDemo(kernel)
        };

        var directionsDemo = new RibbonButtonViewModel()
        {
            Title = "Direction Api Demo",
            Id = DebugRibbonIds.DirectionsDemo,
            EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Tombstone.png",
            DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Tombstone_GS.png",
            IsEnabled = true,
            Callback = () => OpenDirectionsDemo(kernel)
        };

        var jiraDemo = new RibbonButtonViewModel()
        {
            Title = "Jira Api Demo",
            Id = DebugRibbonIds.JiraDemo,
            EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Tombstone.png",
            DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Tombstone_GS.png",
            IsEnabled = true,
            Callback = async () => await OpenJiraDemo(kernel)
        };

        // Desting & Design
        ribbonService.AddButton(bindingDemo);
        ribbonService.AddButton(bindingDemo2);
        ribbonService.AddButton(bindingDemo3);
        ribbonService.AddButton(userControlDemo);
        ribbonService.AddButton(templatePractice);

        ribbonService.AddButton(exceptionDemo);

        // Functionality Demo
        ribbonService.AddButton(asyncBusyDemo);
        ribbonService.AddButton(directionsDemo);
        ribbonService.AddButton(jiraDemo);
    }

    private static void OpenBindingDemo(IKernel kernel)
    {
        var bindingDemo = kernel.Get<BindingDemoViewModel>();
        var windowManager = kernel.Get<IWindowManager>();

        windowManager.ShowDialogAsync(bindingDemo);
    }

    private static void OpenBindingDemo2(IKernel kernel)
    {
        var bindingDemo2 = kernel.Get<BindingDemo2ViewModel>();
        var windowManager = kernel.Get<IWindowManager>();

        windowManager.ShowDialogAsync(bindingDemo2);
    }

    private static void OpenBindingDemo3(IKernel kernel)
    {
        var bindingDemo3 = kernel.Get<BindingDemo3ViewModel>();
        var windowManager = kernel.Get<IWindowManager>();

        windowManager.ShowDialogAsync(bindingDemo3);
    }

    private static void OpenUserControlDemo(IKernel kernel)
    {
        var userControlDemo = kernel.Get<UserControlDemoViewModel>();
        var windowManager = kernel.Get<IWindowManager>();

        windowManager.ShowWindowAsync(userControlDemo);
    }

    private static void OpenAsyncBusyDemo(IKernel kernel)
    {
        var asyncBusyDemo = kernel.Get<AsyncBusyDemoViewModel>();
        var documentService = kernel.Get<IDocumentService>();

        documentService.OpenDocument(asyncBusyDemo);
    }

    private static void OpenDirectionsDemo(IKernel kernel)
    {
        var routePlannerDemo = kernel.Get<DirectionsDemoViewModel>();
        var windowManager = kernel.Get<IWindowManager>();

        windowManager.ShowWindowAsync(routePlannerDemo);
    }

    private static async Task OpenJiraDemo(IKernel kernel)
    {
        var jiraApiService = kernel.Get<IJiraApiService>();
        var issues = await jiraApiService.GetAllReadyForDemoJiraIssuesForEmployee(
            "https://jira.endpoint",
            "fName.lName",
            "password",
            "fName.lName");

        if (issues.IsFailure)
        {
            var dialogService = kernel.Get<IDialogService>();
            dialogService.ShowToast(issues.Error, ToastType.Error);

            return;
        }

        foreach (var issue in issues.Value)
        {
            Console.WriteLine(issue);
        }
    }

    private static void OpenTemplatePractice(IKernel kernel)
    {
        var userControlDemo = kernel.Get<TemplatePracticeViewModel>();
        var windowManager = kernel.Get<IWindowManager>();

        windowManager.ShowWindowAsync(userControlDemo);
    }
}
