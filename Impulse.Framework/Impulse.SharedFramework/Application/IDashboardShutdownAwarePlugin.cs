// <copyright file="IDashboardShutdownAwarePlugin.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace Impulse.SharedFramework.Application;

public interface IDashboardShutdownAwarePlugin
{
    Task OnShutdownRequested();
}
