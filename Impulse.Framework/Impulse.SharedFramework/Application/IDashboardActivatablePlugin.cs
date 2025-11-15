// <copyright file="IDashboardActivatablePlugin.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace Impulse.SharedFramework.Application;

public interface IDashboardActivatablePlugin
{
    Task OnDashboardActivated();

    Task OnDashboardSuspended();
}
