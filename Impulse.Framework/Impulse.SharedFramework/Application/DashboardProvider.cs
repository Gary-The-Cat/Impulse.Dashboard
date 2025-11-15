// <copyright file="DashboardProvider.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Caliburn.Micro;
using CSharpFunctionalExtensions;
using Impulse.Repository.Persistent;
using Impulse.SharedFramework.Services;

namespace Impulse.SharedFramework.Plugin;
public interface IDashboardProvider
{
    public IWindowManager WindowManager { get; }

    public IRibbonService RibbonService { get; }

    public IDocumentService DocumentService { get; }

    public IToolWindowService ToolWindowService { get; }

    public IWorkflowService WorkflowService { get; }

    public IProjectExplorerService ProjectExplorerService { get; }

    public IDialogService DialogService { get; }

    public IPluginDataRepository PluginDataRepository { get; }

    public Result RegisterRequiredService<T>();

    public T GetService<T>();
}
