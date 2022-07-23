// <copyright file="IApplication.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using Impulse.SharedFramework.Plugin;

namespace Impulse.SharedFramework.Application;

public interface IApplication
{
    string DisplayName { get; }

    Uri Icon { get; }

    IDashboardProvider Dashboard { get; set; }

    Task LaunchApplication();

    Task Initialize();

    Task OnClose();
}
