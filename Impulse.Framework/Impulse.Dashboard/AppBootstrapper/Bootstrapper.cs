// <copyright file="Bootstrapper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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
using Impulse.Framework.Dashboard.AppBootstrapper;
using Impulse.Framework.Dashboard.Configuration.Ribbon;
using Impulse.Framework.Dashboard.Providers;
using Impulse.Logging.Contracts;
using Impulse.Logging.Domain.Services;
using Impulse.Logging.UI.LogWindow;
using Impulse.Repository.Persistent;
using Impulse.Shared.ExtensionMethods;
using Impulse.Shared.Interfaces;
using Impulse.SharedFramework.Application;
using Impulse.SharedFramework.Plugin;
using Impulse.SharedFramework.ProjectExplorer;
using Impulse.SharedFramework.Services;
using Impulse.SharedFramework.Services.Layout;
using Impulse.SharedFramework.Shell;
using Microsoft.Win32;
using Ninject;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;

namespace Impulse.Dashboard.AppBootstrapper;

public class Bootstrapper : BootstrapperBase
{
    private const string ApplicationRegistryPath = @"SOFTWARE\Tutorials With Gary\Applications";
    private const string PluginRegistryPath = @"SOFTWARE\Tutorials With Gary\Plugins";
    private const string DebugPluginPath = @"C:\Users\luke.berry\Documents\GitHub\Wordle.Solver\Application\Debug\net7.0-windows";

    private IDashboardProvider dashboard;
    private readonly List<IPlugin> loadedPlugins = new();
    private ILogService logService;
    private string currentThemeName = "Light.Blue";

    public Bootstrapper()
    {
        var args = Environment.GetCommandLineArgs();

        ApplicationPaths = GetPathsFromIndices(GetIndicesForTag(args, "--application"), args);
        PluginPaths = GetPathsFromIndices(GetIndicesForTag(args, "--plugin"), args);

        // When launching from debug, command arguments are used to launch the application being debugged.
        // If no paths are provided, the app selection window should be shown to the user.
        bool isAppSelectionLaunch = !ApplicationPaths.Any() && !PluginPaths.Any();
        if (isAppSelectionLaunch)
        {
            var applicationKey = Registry.CurrentUser.OpenSubKey(ApplicationRegistryPath);
            if (applicationKey != null)
            {
                ApplicationPaths.AddRange(applicationKey.GetValueNames().Select(applicationSubKey =>
                    applicationKey.GetValue(applicationSubKey).ToString()));
            }

            PluginPaths.Add(DebugPluginPath);

            var pluginKey = Registry.CurrentUser.OpenSubKey(PluginRegistryPath);
            if (pluginKey != null)
            {
                PluginPaths.AddRange(pluginKey.GetValueNames().Select(pluginSubKey =>
                    pluginKey.GetValue(pluginSubKey).ToString()));
            }
        }

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

        // Initialize our Kernel
        InitializeKernel();

        // Initialize the Ribbon
        InitializeRibbon();

        // Load all of the plugins (supports 0 -> 1 applications)
        InitializeApplications();

        // Load all of the plugins (supports 0 -> n applications)
        InitializePlugins();

        // Load the theme
        InitializeTheme();

        base.Initialize();
    }

    protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        logService ??= Kernel.Get<ILogService>();
        LogError($"Unhandled exception captured at {DateTime.Now:T}", e.Exception);

        var dialogService = Kernel.Get<IDialogService>();
        dialogService.ShowException("Unhandled Exception", e.Exception.Message);

        e.Handled = true;
    }

    protected override async void OnStartup(object _, StartupEventArgs __)
    {
        // Create the startup window
        var shellViewModel = Kernel.Get<IShellViewModel>();
        var windowManager = Kernel.Get<IWindowManager>();

        await windowManager.ShowWindowAsync(shellViewModel);
        LogInfo("Shell window initialised.");
        RegisterShellWindowEvents();
        await NotifyDashboardActivatedAsync();

        if (ActiveApplication == null && Applications != null)
        {
            LogInfo($"Prompting user to select one of {Applications.Count()} discovered applications.");
            var applicationInstances = new List<IApplication>();
            foreach (var applicationType in Applications)
            {
                IApplication applicationInstance = null;

                try
                {
                    applicationInstance = (IApplication)Activator.CreateInstance(applicationType);
                }
                catch (Exception e)
                {
                    LogError($"Failed to create application instance for {applicationType.FullName}.", e);
                    continue;
                }

                applicationInstance.Dashboard = this.dashboard;
                applicationInstances.Add(applicationInstance);
            }

            var applicationSelectView = new ApplicationSelectView();
            var applicationSelectViewModel = new ApplicationSelectViewModel(applicationInstances);
            applicationSelectView.DataContext = applicationSelectViewModel;

            applicationSelectView.ShowDialog();

            ActiveApplication = applicationSelectViewModel.SelectedApplication;
            if (ActiveApplication != null)
            {
                LogInfo($"User selected application {ActiveApplication.DisplayName}.");
            }
            else
            {
                LogWarning("Application selection dialog closed without selecting an application.");
            }
        }

        // If there was an application in the target folder, load it.
        if (ActiveApplication != null)
        {
            try
            {
                LogInfo($"Initialising application {ActiveApplication.DisplayName}.");
                await ActiveApplication.Initialize();
                Application.Current.MainWindow.Title = ActiveApplication.DisplayName;
                shellViewModel.ActiveApplication = ActiveApplication;
                await NotifyActiveApplicationChangedAsync(ActiveApplication);

                LogInfo($"Launching application {ActiveApplication.DisplayName}.");
                await ActiveApplication.LaunchApplication();
                LogInfo($"Application {ActiveApplication.DisplayName} launched successfully.");
            }
            catch (Exception ex)
            {
                LogError($"Application {ActiveApplication.DisplayName} failed during startup.", ex);
                throw;
            }
        }
        else
        {
            // We were unable to load an application
            Application.Current.MainWindow.Title = "Dashboard";
            LogWarning("No active application was launched; dashboard shell will stay idle.");
        }
    }

    protected override void OnExit(object sender, EventArgs e)
    {
        LogInfo("Dashboard shutting down.");
        NotifyShutdownRequestedAsync().GetAwaiter().GetResult();
        ClosePlugins().GetAwaiter().GetResult();

        if (ActiveApplication != null)
        {
            ActiveApplication.OnClose().GetAwaiter().GetResult();
        }

        base.OnExit(sender, e);
    }

    protected override IEnumerable<Assembly> SelectAssemblies()
    {
        var types = PluginLoader.GetAllTypes(Directory.GetCurrentDirectory()).ToList();
        types.AddRange(PluginLoader.GetAllInstances<IPlugin>(PluginPaths));
        types.AddRange(PluginLoader.GetAllInstances<IApplication>(ApplicationPaths));

        var coreAssemblies = new[]
        {
            Assembly.GetExecutingAssembly(),
            typeof(LogWindowViewModel).Assembly,
        };

        return coreAssemblies.Concat(types.Select(i => i.assembly)).Distinct();
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
        ThemeManager.Current.ChangeTheme(((RibbonService)RibbonService).GetRibbonControl(), currentThemeName);

        var shellViewModel = (ShellViewModel)Kernel.Get<IShellViewModel>();
        shellViewModel.Theme = new DockLightTheme();
        shellViewModel.NotifyOfPropertyChange("Theme");
        NotifyThemeChangedAsync(currentThemeName).GetAwaiter().GetResult();
    }

    private void InitializeApplications()
    {
        var applications = PluginLoader.GetAllInstances<IApplication>(this.ApplicationPaths).ToList();
        LogInfo($"Application discovery complete. Paths searched: {ApplicationPaths.Count}. Candidates found: {applications.Count}.");

        if (!applications.Any())
        {
            LogWarning("No applications were discovered in the supplied paths.");
            return;
        }

        if (applications.Count > 1)
        {
            Applications = applications.Select(a => a.type).ToList();
            LogInfo($"Multiple applications available: {string.Join(", ", Applications.Select(t => t.FullName))}");
            return;
        }

        var applicationReference = applications.First();
        try
        {
            LogInfo($"Instantiating application {applicationReference.type.FullName} from {applicationReference.assembly.Location}.");
            ActiveApplication = (IApplication)Activator.CreateInstance(applicationReference.type);
            ActiveApplication.Dashboard = this.dashboard;
            LogInfo($"Application {ActiveApplication.DisplayName} instantiated successfully.");
        }
        catch (Exception ex)
        {
            LogError($"Failed to instantiate application {applicationReference.type.FullName}.", ex);
        }
    }

    private void InitializePlugins()
    {
        var plugins = PluginLoader.GetAllInstances<IPlugin>(this.PluginPaths).ToList();
        LogInfo($"Plugin discovery complete. Paths searched: {PluginPaths.Count}. Candidates found: {plugins.Count}.");

        foreach (var plugin in plugins)
        {
            try
            {
                LogInfo($"Loading plugin {plugin.type.FullName} from {plugin.assembly.Location}.");
                var instance = (IPlugin)Activator.CreateInstance(plugin.type);
                instance.Dashboard = this.dashboard;
                instance.Initialize();
                loadedPlugins.Add(instance);
                LogInfo($"Plugin {plugin.type.FullName} initialised.");
            }
            catch (Exception ex)
            {
                LogError($"Failed to initialise plugin {plugin.type.FullName}.", ex);
            }
        }
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
        Kernel.Bind<IPluginDataRepository>().To<PluginDataRepository>().InSingletonScope();
        Kernel.Bind<ILogService>().To<LogService>().InSingletonScope()
            .WithConstructorArgument("dateTimeProvider", new DateTimeProvider());
        Kernel.Bind<IDocumentService>().To<DocumentService>().InSingletonScope();
        Kernel.Bind<IToolWindowService>().To<ToolWindowService>().InSingletonScope();
        Kernel.Bind<IWorkflowService>().To<WorkflowService>().InSingletonScope();
        Kernel.Bind<IProjectExplorerService>().To<ProjectExplorerViewModel>().InSingletonScope();
        Kernel.Bind<IDialogService>().To<DialogService>().InSingletonScope()
            .WithConstructorArgument("notifier", CreateDefaultNotifier())
            .WithConstructorArgument("shell", Kernel.Get<IShellViewModel>());

        BindKernelInjectedTypes();
        logService = Kernel.Get<ILogService>();
        LogStartupMetadata();

        this.dashboard = new DashboardProvider(Kernel);

        var toolWindowService = Kernel.Get<IToolWindowService>();

        if (Kernel.Get<IProjectExplorerService>() is ToolWindowBase projectExplorerToolWindow)
        {
            toolWindowService.OpenLeftPaneToolWindow(projectExplorerToolWindow);
        }

        toolWindowService.OpenBottomPaneToolWindow(Kernel.Get<LogWindowViewModel>());
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
        return new Notifier(config =>
        {
            config.PositionProvider = new WindowPositionProvider(
                parentWindow: Application.Current.MainWindow,
                corner: Corner.BottomRight,
                offsetX: 10,
                offsetY: 10);

            config.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(3),
                maximumNotificationCount: MaximumNotificationCount.FromCount(5));

            config.Dispatcher = Application.Current.Dispatcher;
        });
    }

    private void LogInfo(string message) =>
        _ = logService?.LogInfo(message);

    private void LogWarning(string message) =>
        _ = logService?.LogWarning(message);

    private void LogError(string message, Exception exception) =>
        _ = logService?.LogException(message, exception);

    private void LogStartupMetadata()
    {
        var args = string.Join(' ', Environment.GetCommandLineArgs());
        LogInfo($"Dashboard starting with arguments: {args}");
        LogPaths("Application search paths", ApplicationPaths);
        LogPaths("Plugin search paths", PluginPaths);
    }

    private void LogPaths(string label, IEnumerable<string> paths)
    {
        if (paths == null)
        {
            return;
        }

        LogInfo($"{label}: {string.Join(", ", paths)}");
    }

    private void InitializeRibbon()
    {
        RibbonService = Kernel.Get<IRibbonService>();
        if (ShouldLoadDebugRibbon())
        {
            DebugTabLoader.LoadDebuggerTab(Kernel, RibbonService);
        }
        ConfigurationRibbon.LoadConfigTab(RibbonService);
    }

    private void RegisterShellWindowEvents()
    {
        var mainWindow = Application.Current.MainWindow;
        if (mainWindow == null)
        {
            return;
        }

        mainWindow.Activated += MainWindow_Activated;
        mainWindow.Deactivated += MainWindow_Deactivated;
    }

    private async void MainWindow_Activated(object sender, EventArgs e)
    {
        await NotifyDashboardActivatedAsync();
    }

    private async void MainWindow_Deactivated(object sender, EventArgs e)
    {
        await NotifyDashboardSuspendedAsync();
    }

    private Task NotifyDashboardActivatedAsync()
    {
        return NotifyPluginsAsync<IDashboardActivatablePlugin>(plugin => plugin.OnDashboardActivated());
    }

    private Task NotifyDashboardSuspendedAsync()
    {
        return NotifyPluginsAsync<IDashboardActivatablePlugin>(plugin => plugin.OnDashboardSuspended());
    }

    private Task NotifyThemeChangedAsync(string themeName)
    {
        return NotifyPluginsAsync<IDashboardThemeAwarePlugin>(plugin => plugin.OnThemeChanged(themeName));
    }

    private Task NotifyActiveApplicationChangedAsync(IApplication application)
    {
        return NotifyPluginsAsync<IApplicationAwarePlugin>(plugin => plugin.OnActiveApplicationChanged(application));
    }

    private Task NotifyShutdownRequestedAsync()
    {
        return NotifyPluginsAsync<IDashboardShutdownAwarePlugin>(plugin => plugin.OnShutdownRequested());
    }

    private Task ClosePlugins()
    {
        var closeTasks = loadedPlugins.Select(plugin => plugin.OnClose());
        return Task.WhenAll(closeTasks);
    }

    private Task NotifyPluginsAsync<TPlugin>(Func<TPlugin, Task> callback)
        where TPlugin : class
    {
        var tasks = loadedPlugins
            .OfType<TPlugin>()
            .Select(callback)
            .ToList();

        return tasks.Any() ? Task.WhenAll(tasks) : Task.CompletedTask;
    }

    private static bool ShouldLoadDebugRibbon()
    {
#if DEBUG
        return true;
#else
        if (Debugger.IsAttached)
        {
            return true;
        }

        var envValue = Environment.GetEnvironmentVariable("IMPULSE_ENABLE_DEBUG_RIBBON");
        if (string.IsNullOrWhiteSpace(envValue))
        {
            return false;
        }

        return string.Equals(envValue, "1", StringComparison.OrdinalIgnoreCase)
            || string.Equals(envValue, "true", StringComparison.OrdinalIgnoreCase);
#endif
    }
}
