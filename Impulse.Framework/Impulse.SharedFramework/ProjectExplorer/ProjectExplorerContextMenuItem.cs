// <copyright file="ProjectExplorerContextMenuItem.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;

namespace Impulse.SharedFramework.ProjectExplorer;

public class ProjectExplorerContextMenuItem
{
    public Uri Image { get; set; }

    public string Title { get; set; }

    public Action Callback { get; set; }
}
