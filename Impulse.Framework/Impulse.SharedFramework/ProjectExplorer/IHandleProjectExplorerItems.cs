// <copyright file="IHandleProjectExplorerItems.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Impulse.Shared.Interfaces;
using Impulse.SharedFramework.ProjectExplorer;
using Impulse.SharedFramework.Services.Layout;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Impulse.Shared.ProjectExplorer;

public interface IHandleProjectExplorerItems : IAmKernelInjected
{
    bool CanHandle<T>(T t) where T : ProjectExplorerItemBase;

    Task Open<T>(T t) where T : ProjectExplorerItemBase;

    IEnumerable<ProjectExplorerContextMenuItem> GetContextMenuItems<T>(T t) where T : ProjectExplorerItemBase;
}
