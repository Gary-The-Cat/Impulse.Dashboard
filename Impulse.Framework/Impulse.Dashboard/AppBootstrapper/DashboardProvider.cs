using System;
using System.Collections.Generic;
using Caliburn.Micro;
using CSharpFunctionalExtensions;
using Impulse.Repository.Persistent;
using Impulse.Shared.Exceptions;
using Impulse.Shared.ExtensionMethods;
using Impulse.SharedFramework.Plugin;
using Impulse.SharedFramework.Services;
using Ninject;

namespace Impulse.Framework.Dashboard.AppBootstrapper;

internal class DashboardProvider : IDashboardProvider
{
    private WeakReference<IKernel> kernelReference;

    private HashSet<Type> registeredServices;

    public DashboardProvider(IKernel kernel)
    {
        this.kernelReference = new WeakReference<IKernel>(kernel);
        registeredServices = new HashSet<Type>();
    }

    private IKernel Kernel => kernelReference.Value();

    // Core services
    public IWindowManager WindowManager => this.Kernel.Get<IWindowManager>();

    public IRibbonService RibbonService => this.Kernel.Get<IRibbonService>();

    public IDocumentService DocumentService => this.Kernel.Get<IDocumentService>();

    public IToolWindowService ToolWindowService => this.Kernel.Get<IToolWindowService>();

    public IWorkflowService WorkflowService => this.Kernel.Get<IWorkflowService>();

    public IProjectExplorerService ProjectExplorerService => this.Kernel.Get<IProjectExplorerService>();

    public IDialogService DialogService => this.Kernel.Get<IDialogService>();

    public IPluginDataRepository PluginDataRepository
    {
        get
        {
            if (!registeredServices.Contains(typeof(IPluginDataRepository)))
            {
                var result = RegisterRequiredService<IPluginDataRepository>();
                if (result.IsFailure)
                {
                    throw new InvalidOperationException(result.Error);
                }
            }

            return this.Kernel.Get<IPluginDataRepository>();
        }
    }

    public Result RegisterRequiredService<T>()
    {
        var tType = typeof(T);

        if (!tType.IsInterface)
        {
            return Result.Failure(
                $"Services must be registed as interfaces. '{tType.Name}' is not an interface.");
        }

        // Get All should not be used here, rather we should be searching paths for implementations
        // of T
        var tServices = this.Kernel.GetAll<T>();
        if (tServices.None())
        {
            return Result.Failure(
                $"The service '{tType.Name}' was not selected when installing the Dashboard." +
                $"Please run the 'Repair' functionality in the installer and include the module.");
        }

        registeredServices.Add(tType);

        return Result.Success();
    }

    public T GetService<T>()
    {
        var type = typeof(T);
        if (!registeredServices.Contains(type))
        {
            throw new UnregisteredServiceException(nameof(T));
        }

        return this.Kernel.Get<T>();
    }
}
