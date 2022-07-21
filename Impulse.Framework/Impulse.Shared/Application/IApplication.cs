// <copyright file="IApplication.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Impulse.Shared.Application;

public interface IApplication
{
    string DisplayName { get; }

    Uri Icon { get; }

    IEnumerable<Type> GetRequiredServices();

    Task LaunchApplication();

    Task Initialize();

    Task OnClose();
}
