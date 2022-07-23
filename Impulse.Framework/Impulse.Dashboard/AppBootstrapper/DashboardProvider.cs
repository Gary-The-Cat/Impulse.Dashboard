using System;
using System.Collections.Generic;
using Caliburn.Micro;
using CSharpFunctionalExtensions;
using Impulse.Shared.Application;
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
    public IWindowManager WindowManager => this.registeredServices.Contains(typeof(IWindowManager))
        ? this.Kernel.Get<IWindowManager>()
        : throw new UnregisteredServiceException(nameof(IWindowManager));

    public IRibbonService RibbonService => this.registeredServices.Contains(typeof(IRibbonService))
        ? this.Kernel.Get<IRibbonService>()
        : throw new UnregisteredServiceException(nameof(IRibbonService));

    public IDocumentService DocumentService => this.registeredServices.Contains(typeof(IDocumentService))
        ? this.Kernel.Get<IDocumentService>()
        : throw new UnregisteredServiceException(nameof(IDocumentService));

    public IToolWindowService ToolWindowService => this.registeredServices.Contains(typeof(IToolWindowService))
        ? this.Kernel.Get<IToolWindowService>()
        : throw new UnregisteredServiceException(nameof(IToolWindowService));

    public IWorkflowService WorkflowService => this.registeredServices.Contains(typeof(IWorkflowService))
        ? this.Kernel.Get<IWorkflowService>()
        : throw new UnregisteredServiceException(nameof(IWorkflowService));

    public IProjectExplorerService ProjectExplorerService => this.registeredServices.Contains(typeof(IProjectExplorerService))
        ? this.Kernel.Get<IProjectExplorerService>()
        : throw new UnregisteredServiceException(nameof(IProjectExplorerService));

    public IDialogService DialogService => this.registeredServices.Contains(typeof(IDialogService))
        ? this.Kernel.Get<IDialogService>()
        : throw new UnregisteredServiceException(nameof(IDialogService));

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
