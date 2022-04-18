// <copyright file="WorkflowTabBadgeViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using Impulse.Shared.ReactiveUI;

namespace Impulse.Dashboard.Services.Workflow.WorkflowTabBadge;

public class WorkflowTabBadgeViewModel : ReactiveViewModelBase
{
    public Uri Image { get; set; }

    public string DisplayName { get; set; }

    public string Details { get; set; }

    public bool IsSelected { get; set; }
}
