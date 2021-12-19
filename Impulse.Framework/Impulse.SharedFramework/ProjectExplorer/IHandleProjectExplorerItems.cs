// <copyright file="IHandleProjectExplorerItems.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using Impulse.Shared.Interfaces;
using Impulse.SharedFramework.Services.Layout;

namespace Impulse.SharedFramework.ProjectExplorer
{
    public interface IHandleProjectExplorerItems : IAmKernelInjected
    {
        bool CanHandle<T>(T t) where T : ProjectExplorerItemBase;

        Task Open<T>(T t) where T : ProjectExplorerItemBase;

        IEnumerable<ProjectExplorerContextMenuItem> GetContextMenuItems<T>(T t) where T : ProjectExplorerItemBase;
    }
}
