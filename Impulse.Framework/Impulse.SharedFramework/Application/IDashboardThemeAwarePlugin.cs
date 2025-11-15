// <copyright file="IDashboardThemeAwarePlugin.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace Impulse.SharedFramework.Application;

public interface IDashboardThemeAwarePlugin
{
    Task OnThemeChanged(string themeName);
}
