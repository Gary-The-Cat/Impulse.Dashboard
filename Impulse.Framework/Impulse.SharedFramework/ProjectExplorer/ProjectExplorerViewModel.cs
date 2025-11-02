// <copyright file="ProjectExplorerViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Impulse.Shared.Enums;
using Impulse.Shared.ProjectExplorer;
using Impulse.SharedFramework.Services;
using Impulse.SharedFramework.Services.Layout;
using Impulse.SharedFramework.Utilities;

namespace Impulse.SharedFramework.ProjectExplorer;

public class ProjectExplorerViewModel : ToolWindowBase, IProjectExplorerService
{
    private readonly IHandleProjectExplorerItems[] handlers;
    private readonly IDialogService dialogService;

    public ProjectExplorerViewModel(
        IHandleProjectExplorerItems[] handlers,
        IDialogService dialogService)
    {
        DisplayName = "Explorer";
        Placement = ToolWindowPlacement.Left;

        this.handlers = handlers;
        this.dialogService = dialogService;

        Items = new ObservableCollection<ProjectExplorerItemBase>();

        SelectedItems = new ObservableCollection<ProjectExplorerItemBase>();
    }

    public ObservableCollection<ProjectExplorerItemBase> Items { get; set; }

    public ObservableCollection<ProjectExplorerItemBase> SelectedItems { get; set; }

    public bool IsSelected { get; set; }

    public Task AddItemAsync(ProjectExplorerItemBase item)
    {
        var handler = handlers.FirstOrDefault(h => h.CanHandle(item));

        item.PopulateContextMenu(handler);
        item.AddContextMenuItems(ProjectExplorerDefaults.GetDefaultItems(item, this));

        if (string.IsNullOrWhiteSpace(item.Icon))
        {
            item.Icon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Circle.png";
        }

        if (string.IsNullOrWhiteSpace(item.OriginalDisplayName))
        {
            item.OriginalDisplayName = item.DisplayName;
        }

        if (item.ParentId != Guid.Empty)
        {
            var parent = FindItem(item.ParentId, Items);

            if (parent == null)
            {
                throw new InvalidOperationException($"A parent item with Id '{item.ParentId}' could not be found.");
            }

            parent.Items.Add(item);
        }
        else
        {
            // Add the item to the root
            Items.Add(item);
        }

        return Task.CompletedTask;
    }

    public void ClearAllItems()
    {
        Items.Clear();
    }

    public Task DeleteItemAsync(Guid itemId)
    {
        var removedItems = new List<ProjectExplorerItemBase>();

        if (TryRemoveItem(itemId, Items, removedItems))
        {
            foreach (var item in removedItems)
            {
                if (SelectedItems.Contains(item))
                {
                    SelectedItems.Remove(item);
                }
            }
        }

        return Task.CompletedTask;
    }

    public async Task DeleteItemsAsync(IEnumerable<ProjectExplorerItemBase> items)
    {
        if (items == null)
        {
            return;
        }

        var candidates = items
            .Where(item => item != null)
            .Distinct()
            .OrderByDescending(GetDepth)
            .ToList();

        if (!candidates.Any())
        {
            return;
        }

        var messageTarget = candidates.Count == 1
            ? $"'{candidates[0].DisplayName}'"
            : $"{candidates.Count} items";

        var result = await dialogService.ShowConfirmation(
            "Confirm Delete",
            $"Are you sure you want to delete {messageTarget}?");

        if (result != DialogResult.Yes)
        {
            return;
        }

        foreach (var candidate in candidates)
        {
            await DeleteItemAsync(candidate.Id);
        }
    }

    public async Task<ProjectExplorerFolder> CreateFolderAsync(ProjectExplorerItemBase parent)
    {
        if (parent == null)
        {
            throw new ArgumentNullException(nameof(parent));
        }

        var resolvedParent = FindItem(parent.Id, Items);

        if (resolvedParent == null)
        {
            throw new InvalidOperationException($"A parent item with Id '{parent.Id}' could not be found.");
        }

        var folderName = UniqueNameGenerator.GenerateUniqueName(
            resolvedParent.Items
                .Where(i => i != null && !string.IsNullOrWhiteSpace(i.DisplayName))
                .Select(i => i.DisplayName!.Trim()),
            "New Folder",
            UniqueNameStrategy.NumberInParentheses,
            "New Folder");

        var folder = new ProjectExplorerFolder
        {
            ParentId = resolvedParent.Id,
            DisplayName = folderName,
            IsExpanded = false,
            IsEditing = true,
            IsEditable = true,
            OriginalDisplayName = folderName
        };

        resolvedParent.IsExpanded = true;

        await AddItemAsync(folder);

        SelectedItems.Clear();
        SelectedItems.Add(folder);
        folder.IsSelected = true;

        return folder;
    }

    public Task BeginRenameAsync(ProjectExplorerItemBase item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        var resolvedItem = FindItem(item.Id, Items) ?? item;

        resolvedItem.OriginalDisplayName = resolvedItem.DisplayName;
        resolvedItem.IsEditable = true;
        resolvedItem.IsEditing = true;
        resolvedItem.IsSelected = true;

        SelectedItems.Clear();
        SelectedItems.Add(resolvedItem);

        return Task.CompletedTask;
    }

    public void CommitRename(ProjectExplorerItemBase item, string proposedName)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        var resolvedItem = FindItem(item.Id, Items) ?? item;
        var siblings = GetSiblingCollection(resolvedItem);

        var fallback = string.IsNullOrWhiteSpace(resolvedItem.OriginalDisplayName)
            ? "New Item"
            : resolvedItem.OriginalDisplayName;

        var siblingNames = siblings
            .Where(sibling => sibling != null && sibling.Id != resolvedItem.Id && !string.IsNullOrWhiteSpace(sibling.DisplayName))
            .Select(sibling => sibling.DisplayName!.Trim());

        var uniqueName = UniqueNameGenerator.GenerateUniqueName(
            siblingNames,
            proposedName,
            UniqueNameStrategy.NumberInParentheses,
            fallback);

        resolvedItem.DisplayName = uniqueName;
        resolvedItem.OriginalDisplayName = uniqueName;
        resolvedItem.IsEditing = false;
        resolvedItem.IsEditable = true;
        resolvedItem.IsSelected = true;

        SelectedItems.Clear();
        SelectedItems.Add(resolvedItem);
    }

    public void OpenItem(ProjectExplorerItemBase item)
    {
        var handler = handlers.FirstOrDefault(h => h.CanHandle(item));

        if (handler != null)
        {
            handler.Open(item);
        }
    }

    public IEnumerable<ProjectExplorerItemBase> GetItems()
    {
        foreach (var item in Items)
        {
            yield return item;
        }
    }

    public IEnumerable<ProjectExplorerItemBase> GetItemsRecursive()
    {
        var output = new List<ProjectExplorerItemBase>();
        GetAllItems(Items, ref output);
        return output;
    }

    private List<ProjectExplorerItemBase> GetAllItems(IEnumerable<ProjectExplorerItemBase> items, ref List<ProjectExplorerItemBase> allItems)
    {
        foreach (var item in items)
        {
            item.Index = items.ToList().IndexOf(item);
            GetAllItems(item.Items, ref allItems);
            allItems.Add(item);
        }

        return allItems;
    }

    private ProjectExplorerItemBase? FindItem(Guid itemId, IEnumerable<ProjectExplorerItemBase> items)
    {
        foreach (var item in items)
        {
            if (item.Id == itemId)
            {
                return item;
            }

            var childMatch = FindItem(itemId, item.Items);
            if (childMatch != null)
            {
                return childMatch;
            }
        }

        return null;
    }

    private bool TryRemoveItem(Guid itemId, ObservableCollection<ProjectExplorerItemBase> items, List<ProjectExplorerItemBase> removedItems)
    {
        var item = items.FirstOrDefault(i => i.Id == itemId);

        if (item != null)
        {
            items.Remove(item);
            CollectRemovedItems(item, removedItems);
            removedItems.Add(item);
            return true;
        }

        foreach (var child in items)
        {
            if (TryRemoveItem(itemId, child.Items, removedItems))
            {
                return true;
            }
        }

        return false;
    }

    private void CollectRemovedItems(ProjectExplorerItemBase parent, List<ProjectExplorerItemBase> removedItems)
    {
        foreach (var child in parent.Items)
        {
            CollectRemovedItems(child, removedItems);
            removedItems.Add(child);
        }
    }

    private ProjectExplorerItemBase? GetParent(ProjectExplorerItemBase item)
    {
        if (item.ParentId == Guid.Empty)
        {
            return null;
        }

        return FindItem(item.ParentId, Items);
    }

    private IEnumerable<ProjectExplorerItemBase> GetSiblingCollection(ProjectExplorerItemBase item)
    {
        var parent = GetParent(item);

        return parent?.Items ?? Items;
    }

    private int GetDepth(ProjectExplorerItemBase item)
    {
        var depth = 0;
        var currentParentId = item.ParentId;

        while (currentParentId != Guid.Empty)
        {
            var parent = FindItem(currentParentId, Items);
            if (parent == null)
            {
                break;
            }

            depth++;
            currentParentId = parent.ParentId;
        }

        return depth;
    }
}
