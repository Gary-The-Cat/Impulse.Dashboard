// <copyright file="IPlugin.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using Impulse.SharedFramework.Plugin;
using Impulse.SharedFramework.Services;

namespace Impulse.SharedFramework.Application;

public interface IPlugin
{
    IDashboardProvider Dashboard { get; set; }

    public Task Initialize();

    public Task OnClose();
}