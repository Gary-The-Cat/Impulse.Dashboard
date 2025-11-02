// <copyright file="ProjectExplorerContextMenuItem.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;

namespace Impulse.SharedFramework.ProjectExplorer;

public class ProjectExplorerContextMenuItem
{
    public Uri Image { get; set; }

    public string Title { get; set; }

    public Func<Task> Callback { get; set; }
}
