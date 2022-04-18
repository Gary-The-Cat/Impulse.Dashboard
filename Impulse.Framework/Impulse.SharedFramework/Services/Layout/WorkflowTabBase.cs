// <copyright file="WorkflowTabBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Impulse.Shared.ReactiveUI;

namespace Impulse.SharedFramework.Services.Layout;

public abstract class WorkflowTabBase : ReactiveScreen
{
    public new abstract string DisplayName { get; }

    public abstract string Descrption { get; }
}
