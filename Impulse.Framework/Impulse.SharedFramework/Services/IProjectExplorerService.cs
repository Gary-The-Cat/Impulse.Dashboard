// <copyright file="IProjectExplorerService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Impulse.SharedFramework.Services.Layout;

namespace Impulse.SharedFramework.Services
{
    public interface IProjectExplorerService
    {
        Task AddItemAsync(ProjectExplorerItemBase item);

        Task DeleteItemAsync(Guid itemId);

        IEnumerable<ProjectExplorerItemBase> GetItems();

        IEnumerable<ProjectExplorerItemBase> GetItemsRecursive();

        void ClearAllItems();
    }
}
