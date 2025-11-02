// <copyright file="IProjectExplorerService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Impulse.SharedFramework.Services.Layout;

namespace Impulse.SharedFramework.Services;

public interface IProjectExplorerService
{
    Task AddItemAsync(ProjectExplorerItemBase item);

    Task DeleteItemAsync(Guid itemId);

    Task DeleteItemsAsync(IEnumerable<ProjectExplorerItemBase> items);

    Task<ProjectExplorerFolder> CreateFolderAsync(ProjectExplorerItemBase parent);

    Task BeginRenameAsync(ProjectExplorerItemBase item);

    IEnumerable<ProjectExplorerItemBase> GetItems();

    IEnumerable<ProjectExplorerItemBase> GetItemsRecursive();

    void ClearAllItems();
}
