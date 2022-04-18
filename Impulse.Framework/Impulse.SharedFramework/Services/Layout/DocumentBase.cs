// <copyright file="DocumentBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Impulse.Shared.ExtensionMethods;
using Impulse.Shared.ReactiveUI;
using Ninject;

namespace Impulse.SharedFramework.Services.Layout;

public class DocumentBase : ReactiveScreen
{
    private readonly List<ToolWindowBase> toolWindows;
    private readonly WeakReference<IToolWindowService> toolWindowServiceReference;
    private readonly WeakReference<IKernel> kernelReference;

    public DocumentBase(IKernel kernel)
    {
        toolWindows = new List<ToolWindowBase>();
        toolWindowServiceReference =
            new WeakReference<IToolWindowService>(kernel.Get<IToolWindowService>());
        this.kernelReference = new WeakReference<IKernel>(kernel);
    }

    public IKernel Kernel => kernelReference.Value();

    private IToolWindowService ToolWindowService => toolWindowServiceReference.Value();

    public void AddToolWindow(ToolWindowBase toolWindow)
    {
        toolWindows.Add(toolWindow);
    }

    protected override async Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
    {
        await base.OnDeactivateAsync(close, cancellationToken);

        toolWindows.ForEach(toolWindow => ToolWindowService.HidePaneToolWindow(toolWindow));
    }

    protected override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        await base.OnActivateAsync(cancellationToken);

        toolWindows.ForEach(toolWindow =>
        {
            if (toolWindow.Placement == Shared.Enums.ToolWindowPlacement.Left)
            {
                ToolWindowService.OpenLeftPaneToolWindow(toolWindow);
            }
            else
            {
                ToolWindowService.OpenRightPaneToolWindow(toolWindow);
            }
        });
    }

    protected override void OnViewLoaded(object view)
    {
        base.OnViewLoaded(view);

        this.AcceptChanges();
    }
}
