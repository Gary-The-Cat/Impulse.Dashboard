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
using Impulse.Dashboard.Debug.DemoScreens.GoogleApiDemo;
using Impulse.Dashboard.Debug.DemoScreens.GoogleSuggestionApiDemo;
using Impulse.Dashboard.Debug.DemoScreens.MonoViewerDemo;
using Impulse.Dashboard.Debug.DemoScreens.RoutePlannerDemo;
using Impulse.Dashboard.Debug.DemoScreens.TemplatePractice;
using Impulse.Dashboard.Debug.DemoScreens.UserControlDemo;
using Impulse.Dashboard.Debug.DemoScreens.ViewerDemo.ResidentialView;
using Impulse.Dashboard.Debug.DemoScreens.WorkflowDemo;
using Impulse.Framework.Dashboard.Debug.DemoScreens.WordChecker;
using Impulse.SFML.Viewer.Viewer;
using Impulse.Shared.Services;
using Impulse.SharedFramework.Ribbon;
using Impulse.SharedFramework.Services;
using Impulse.SharedFramework.ToastNotifications;
using Ninject;

namespace Impulse.Dashboard.Debug
{
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

            var taylorDemo = new RibbonButton()
            {
                Title = "SFML Demo",
                Id = DebugRibbonIds.TaylorDemo,
                EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Bat.png",
                DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Bat_GS.png",
                IsEnabled = true,
                Callback = () => OpenSfmlDemo(kernel)
            };

            var bindingDemo = new RibbonButton()
            {
                Title = "Binding Demo",
                Id = DebugRibbonIds.BindingDemo,
                EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Pumpkin.png",
                DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Pumpkin_GS.png",
                IsEnabled = true,
                Callback = () => OpenBindingDemo(kernel)
            };

            var bindingDemo2 = new RibbonButton()
            {
                Title = "Binding Demo 2",
                Id = DebugRibbonIds.BindingDemo2,
                EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Couldren.png",
                DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Couldren_GS.png",
                IsEnabled = true,
                Callback = () => OpenBindingDemo2(kernel)
            };

            var bindingDemo3 = new RibbonButton()
            {
                Title = "Binding Demo 3",
                Id = DebugRibbonIds.BindingDemo3,
                EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Ghost.png",
                DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Ghost_GS.png",
                IsEnabled = true,
                Callback = () => OpenBindingDemo3(kernel)
            };

            var userControlDemo = new RibbonButton()
            {
                Title = "User Control Demo",
                Id = DebugRibbonIds.UserControlDemo,
                EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Monster.png",
                DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Monster_GS.png",
                IsEnabled = true,
                Callback = () => OpenUserControlDemo(kernel)
            };

            var templatePractice = new RibbonButton()
            {
                Title = "Template Practice",
                Id = DebugRibbonIds.TemplatePractice,
                EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Moon.png",
                DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Moon_GS.png",
                IsEnabled = true,
                Callback = () => OpenTemplatePractice(kernel)
            };

            var exceptionDemo = new RibbonButton()
            {
                Title = "Throw Exception",
                Id = DebugRibbonIds.ExceptionDemo,
                EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Coffin.png",
                DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Coffin_GS.png",
                IsEnabled = true,
                Callback = () => throw new System.Exception("You click the exception button")
            };

            var rendererDemo = new RibbonButton()
            {
                Title = "Veldrid Renderer",
                Id = DebugRibbonIds.RendererDemo,
                EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Reaper.png",
                DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Reaper_GS.png",
                IsEnabled = true,
                Callback = () => OpenRendererDemo(kernel)
            };

            var monoRendererDemo = new RibbonButton()
            {
                Title = "Mono Renderer",
                Id = DebugRibbonIds.MonoRendererDemo,
                EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Skull.png",
                DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Skull_GS.png",
                IsEnabled = true,
                Callback = () => OpenMonoRendererDemo(kernel)
            };

            var workflowDemo = new RibbonButton()
            {
                Title = "Workflow",
                Id = DebugRibbonIds.WorkflowDemo,
                EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Workflow.png",
                DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Workflow_GS.png",
                IsEnabled = true,
                Callback = () => OpenWorkflowDemo(kernel)
            };

            var asyncBusyDemo = new RibbonButton()
            {
                Title = "Async Busy",
                Id = DebugRibbonIds.AsyncBusyDemo,
                EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Spider.png",
                DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Spider_GS.png",
                IsEnabled = true,
                Callback = () => OpenAsyncBusyDemo(kernel)
            };

            var wordCheckerDemo = new RibbonButton()
            {
                Title = "Word Checker",
                Id = DebugRibbonIds.WordCheckerDemo,
                EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Spider.png",
                DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Spider_GS.png",
                IsEnabled = true,
                Callback = () => OpenWordCheckerDemo(kernel)
            };

            var googleApiDemo = new RibbonButton()
            {
                Title = "Google Api Demo",
                Id = DebugRibbonIds.GoogleApiDemo,
                EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Tombstone.png",
                DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Tombstone_GS.png",
                IsEnabled = true,
                Callback = () => OpenGoogleApiDemo(kernel)
            };

            var googlePlacesApiDemo = new RibbonButton()
            {
                Title = "Google Suggestion Api",
                Id = DebugRibbonIds.GooglePlacesApiDemo,
                EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Tombstone.png",
                DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Tombstone_GS.png",
                IsEnabled = true,
                Callback = () => OpenGoogleSuggestionApiDemo(kernel)
            };

            var routePlannerDemo = new RibbonButton()
            {
                Title = "Route Planner Demo",
                Id = DebugRibbonIds.RoutePlannerDemo,
                EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Tombstone.png",
                DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Tombstone_GS.png",
                IsEnabled = true,
                Callback = () => OpenRoutePlannerDemo(kernel)
            };

            var directionsDemo = new RibbonButton()
            {
                Title = "Direction Api Demo",
                Id = DebugRibbonIds.DirectionsDemo,
                EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Tombstone.png",
                DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Tombstone_GS.png",
                IsEnabled = true,
                Callback = () => OpenDirectionsDemo(kernel)
            };

            var jiraDemo = new RibbonButton()
            {
                Title = "Jira Api Demo",
                Id = DebugRibbonIds.JiraDemo,
                EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Tombstone.png",
                DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Tombstone_GS.png",
                IsEnabled = true,
                Callback = async () => await OpenJiraDemo(kernel)
            };

            // Desting & Design
            ribbonService.AddButton(taylorDemo);
            ribbonService.AddButton(bindingDemo);
            ribbonService.AddButton(bindingDemo2);
            ribbonService.AddButton(bindingDemo3);
            ribbonService.AddButton(userControlDemo);
            ribbonService.AddButton(templatePractice);

            ribbonService.AddButton(exceptionDemo);

            // Functionality Demo
            ribbonService.AddButton(rendererDemo);
            ribbonService.AddButton(monoRendererDemo);
            ribbonService.AddButton(workflowDemo);
            ribbonService.AddButton(asyncBusyDemo);
            ribbonService.AddButton(googleApiDemo);
            ribbonService.AddButton(googlePlacesApiDemo);
            ribbonService.AddButton(directionsDemo);
            ribbonService.AddButton(jiraDemo);

            ribbonService.AddButton(wordCheckerDemo);

            ribbonService.AddButton(routePlannerDemo);
        }

        private static void OpenSfmlDemo(IKernel kernel)
        {
            var sfmlDemo = kernel.Get<SfmlViewerViewModel>();
            var documentService = kernel.Get<IDocumentService>();

            documentService.OpenDocument(sfmlDemo);
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

        private static void OpenRendererDemo(IKernel kernel)
        {
            var styleDemo = kernel.Get<ViewerDemoViewModel>();
            var windowManager = kernel.Get<IDocumentService>();

            windowManager.OpenDocument(styleDemo);
        }

        private static void OpenMonoRendererDemo(IKernel kernel)
        {
            var styleDemo = kernel.Get<MonoViewerDemoViewModel>();
            var windowManager = kernel.Get<IDocumentService>();

            windowManager.OpenDocument(styleDemo);
        }

        private static void OpenWorkflowDemo(IKernel kernel)
        {
            var workflowService = kernel.Get<IWorkflowService>();

            var workflow = workflowService.CreateWorkflow(
                kernel.Get<ProvideNameWorkflowTabViewModel>(),
                kernel.Get<ProvideAgeWorkflowTabViewModel>(),
                kernel.Get<ProvideNameWorkflowTabViewModel>());

            workflowService.ShowWorkflow(workflow);

            var nameValue = workflow.GetValue<string>("Name");
            var ageValue = workflow.GetValue<int>("Age");
        }

        private static void OpenAsyncBusyDemo(IKernel kernel)
        {
            var asyncBusyDemo = kernel.Get<AsyncBusyDemoViewModel>();
            var documentService = kernel.Get<IDocumentService>();

            documentService.OpenDocument(asyncBusyDemo);
        }

        private static void OpenWordCheckerDemo(IKernel kernel)
        {
            var wordChecker = kernel.Get<WordCheckerViewModel>();
            var documentService = kernel.Get<IDocumentService>();

            documentService.OpenDocument(wordChecker);
        }

        private static void OpenGoogleApiDemo(IKernel kernel)
        {
            var googleApiDemo = kernel.Get<GoogleApiDemoViewModel>();
            var documentService = kernel.Get<IDocumentService>();

            documentService.OpenDocument(googleApiDemo);
        }

        private static void OpenRoutePlannerDemo(IKernel kernel)
        {
            var routePlannerDemo = kernel.Get<RoutePlannerDemoViewModel>();
            var documentService = kernel.Get<IDocumentService>();

            documentService.OpenDocument(routePlannerDemo);
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

        private static void OpenGoogleSuggestionApiDemo(IKernel kernel)
        {
            var googleApiDemo = kernel.Get<GoogleSuggestionApiDemoViewModel>();
            var documentService = kernel.Get<IDocumentService>();

            documentService.OpenDocument(googleApiDemo);
        }

        private static void OpenTemplatePractice(IKernel kernel)
        {
            var userControlDemo = kernel.Get<TemplatePracticeViewModel>();
            var windowManager = kernel.Get<IWindowManager>();

            windowManager.ShowWindowAsync(userControlDemo);
        }
    }
}
