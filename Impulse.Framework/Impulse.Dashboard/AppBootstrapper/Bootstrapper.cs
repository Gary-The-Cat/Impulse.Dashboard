// <copyright file="Bootstrapper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Caliburn.Micro;
using ControlzEx.Theming;
using Impulse.Dashboard.ApplicaitonSelect;
using Impulse.Dashboard.Debug;
using Impulse.Dashboard.Ribbon;
using Impulse.Dashboard.Services;
using Impulse.Dashboard.Services.Workflow;
using Impulse.Dashboard.Shell;
using Impulse.Dashboard.Themes;
using Impulse.Shared.Application;
using Impulse.Shared.ExtensionMethods;
using Impulse.Shared.Interfaces;
using Impulse.Shared.Services;
using Impulse.SharedFramework.Plugin;
using Impulse.SharedFramework.ProjectExplorer;
using Impulse.SharedFramework.Services;
using Impulse.SharedFramework.Shell;
using Ninject;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;

namespace Impulse.Dashboard.AppBootstrapper;

public class Bootstrapper : BootstrapperBase
{
    public Bootstrapper()
    {
        var args = Environment.GetCommandLineArgs();

        ApplicationPaths = GetPathsFromIndices(GetIndicesForTag(args, "--application"), args);
        PluginPaths = GetPathsFromIndices(GetIndicesForTag(args, "--plugin"), args);

        var defaultPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        ApplicationPaths.Add(defaultPath);
        PluginPaths.Add(defaultPath);
        var currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
        var applicationPathsString = string.Join(';', ApplicationPaths);
        var pluginPathsString = string.Join(';', PluginPaths);
        var allString = string.Join(';', new[] { currentPath, applicationPathsString, pluginPathsString });
        Environment.SetEnvironmentVariable("PATH", allString, EnvironmentVariableTarget.Process);
        Initialize();
    }

    public IKernel Kernel { get; private set; }

    public IApplication ActiveApplication { get; private set; }

    public IEnumerable<Type> Applications { get; private set; }

    public IRibbonService RibbonService { get; private set; }

    public List<string> ApplicationPaths { get; }

    public List<string> PluginPaths { get; }

    public new void Initialize()
    {
        // Initialize Caliburn Micro
        InitializeCaliburnMicro();

        // Initialize outr Kernel
        InitializeKernel();

        // Initialize the Ribbon
        InitializeRibbon();

        // Load all of the plugins (supports 0 -> n applications)
        InitializeApplications();

        // Load all of the plugins (supports 0 -> n applications)
        InitializePlugins();

        // Load the theme
        InitializeTheme();

        base.Initialize();
    }

    protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        var dialogService = Kernel.Get<IDialogService>();
        dialogService.ShowException("Unhandled Exception", e.Exception.Message);

        e.Handled = true;
    }

    protected override async void OnStartup(object _, StartupEventArgs __)
    {
        // Create the startup window
        var shellViewModel = Kernel.Get<IShellViewModel>();
        var windowManager = Kernel.Get<IWindowManager>();
        var documentService = Kernel.Get<IDocumentService>();

        await windowManager.ShowWindowAsync(shellViewModel);

        if (ActiveApplication == null && Applications != null)
        {
            var applicationInstances = Applications.Select(a => (IApplication)this.Kernel.Get(a));
            var applicationSelectView = new ApplicationSelectView();
            var applicationSelectViewModel = new ApplicationSelectViewModel(applicationInstances);
            applicationSelectView.DataContext = applicationSelectViewModel;

            applicationSelectView.ShowDialog();

            ActiveApplication = applicationSelectViewModel.SelectedApplication;
        }

        // If there was an application in the target folder, load it.
        if (ActiveApplication != null)
        {
            await ActiveApplication.Initialize();
            Application.Current.MainWindow.Title = ActiveApplication.DisplayName;
            shellViewModel.ActiveApplication = ActiveApplication;

            await ActiveApplication.LaunchApplication();
        }
        else
        {
            // We were unable to load an application
            Application.Current.MainWindow.Title = "Dashboard";
        }
    }

    protected override void OnExit(object sender, EventArgs e)
    {
        if (ActiveApplication != null)
        {
            ActiveApplication.OnClose().GetAwaiter().GetResult();
        }

        base.OnExit(sender, e);
    }

    protected override IEnumerable<Assembly> SelectAssemblies()
    {
        var types = PluginLoader.GetAllTypes(Directory.GetCurrentDirectory());
        types.AddRange(PluginLoader.GetAllInstances<IPlugin>(PluginPaths));
        types.AddRange(PluginLoader.GetAllInstances<IApplication>(ApplicationPaths));

        return new[] { Assembly.GetExecutingAssembly() }.Concat(types.Select(i => i.assembly)).Distinct();
    }

    private IEnumerable<int> GetIndicesForTag(string[] args, string tag)
    {
        return args.IndicesWhere(a => a.Equals(tag)).Select(i => i + 1);
    }

    private List<string> GetPathsFromIndices(IEnumerable<int> indices, string[] args)
    {
        var output = new List<string>();

        foreach (var index in indices)
        {
            if (index < args.Length)
            {
                var path = args[index];
                if (Directory.Exists(path))
                {
                    output.Add(path);
                }
            }
        }

        return output;
    }

    private void InitializeCaliburnMicro()
    {
        // Set the viewLocator to use the kernel to initialize the view.
        // This allows us to inject into view classes.
        ViewLocator.LocateForModelType = (type, _, __) =>
        {
            var t = ViewLocator.LocateTypeForModelType(type, _, __);

            return (UIElement)Kernel.Get(t);
        };
    }

    private void InitializeTheme()
    {
        var lightTheme = new ResourceDictionary();
        lightTheme.Source = new Uri("pack://application:,,,/Impulse.SharedFramework;Component/Themes/LightThemeColours.xaml");

        Application.Current.Resources.MergedDictionaries.Add(lightTheme);
        ThemeManager.Current.ChangeTheme(((RibbonService)RibbonService).GetRibbonControl(), "Light.Blue");

        var shellViewModel = (ShellViewModel)Kernel.Get<IShellViewModel>();
        shellViewModel.Theme = new DockLightTheme();
        shellViewModel.NotifyOfPropertyChange("Theme");
    }

    private void InitializeApplications()
    {
        // Get the plugins from the local plugin folder
        var applications = PluginLoader.GetAllInstances<IApplication>(this.ApplicationPaths);

        // Launch without setting the active application
        if (!applications.Any())
        {
            return;
        }

        // :TODO: Add support for multiple applications within a single dashboard session
        if (applications.Count() > 1)
        {
            Applications = applications.Select(a => a.type).ToList();
        }
        else
        {
            ActiveApplication = (IApplication)this.Kernel.Get(applications.First().type);
        }
    }

    private void InitializePlugins()
    {
        // Get the plugins from the local plugin folder
        var plugins = PluginLoader.GetAllInstances<IPlugin>(this.PluginPaths);

        var ribbonService = Kernel.Get<IRibbonService>();
        var documentService = Kernel.Get<IDocumentService>();

        foreach (var plugin in plugins)
        {
            var instance = (IPlugin)plugin.type.GetMethod("Create").Invoke(
                plugin,
                new object[] { ribbonService, documentService });

            instance.Initialize();
        }
    }

    private bool Filter(Assembly s)
    {
        return s.FullName.StartsWith("Impulse.");
    }

    private void InitializeKernel()
    {
        Kernel = new StandardKernel();
        var assemblies = SelectAssemblies();
        Kernel.Load(assemblies);

        // Bind the shell to the kernel in singleton scope.
        Kernel.Bind<IShellView>().To<ShellView>().InSingletonScope();
        Kernel.Bind<IShellViewModel>().To<ShellViewModel>().InSingletonScope();

        // Bind all services to the kernel
        Kernel.Bind<IWindowManager>().To<WindowManager>().InSingletonScope();
        Kernel.Bind<IRibbonService>().To<RibbonService>().InSingletonScope();
        Kernel.Bind<IDocumentService>().To<DocumentService>().InSingletonScope();
        Kernel.Bind<IToolWindowService>().To<ToolWindowService>().InSingletonScope();
        Kernel.Bind<IWorkflowService>().To<WorkflowService>().InSingletonScope();
        Kernel.Bind<IProjectExplorerService>().To<ProjectExplorerViewModel>().InSingletonScope();
        Kernel.Bind<IDialogService>().To<DialogService>().InSingletonScope()
            .WithConstructorArgument("notifier", CreateDefaultNotifier())
            .WithConstructorArgument("shell", Kernel.Get<IShellViewModel>());

        //Kernel.Bind<IGoogleApiService>().To<GoogleApiService>().InSingletonScope();

        BindKernelInjectedTypes();
    }

    private void BindKernelInjectedTypes()
    {
        foreach (var assembly in this.SelectAssemblies())
        {
            foreach (var type in assembly.GetExportedTypes().Where(t => !t.IsInterface))
            {
                if (type.Implements(typeof(IAmKernelInjected)))
                {
                    var bindingInterface = type.GetInterfaces().First(i => i.Implements(typeof(IAmKernelInjected)));
                    Kernel.Bind(bindingInterface).To(type).InSingletonScope();
                }
            }
        }
    }

    private object CreateDefaultNotifier()
    {
        return new Notifier(cfg =>
        {
            cfg.PositionProvider = new WindowPositionProvider(
                parentWindow: Application.Current.MainWindow,
                corner: Corner.BottomRight,
                offsetX: 10,
                offsetY: 10);

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(3),
                maximumNotificationCount: MaximumNotificationCount.FromCount(5));

            cfg.Dispatcher = Application.Current.Dispatcher;
        });
    }

    private void InitializeRibbon()
    {
        RibbonService = Kernel.Get<IRibbonService>();

#if DEBUG
        DebugTabLoader.LoadDebuggerTab(Kernel, RibbonService);
#endif
    }
}
