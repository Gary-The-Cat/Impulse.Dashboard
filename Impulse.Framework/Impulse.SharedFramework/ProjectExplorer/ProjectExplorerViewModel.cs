// <copyright file="ProjectExplorerViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Impulse.SharedFramework.Services;
using Impulse.SharedFramework.Services.Layout;

namespace Impulse.SharedFramework.ProjectExplorer
{
    public class ProjectExplorerViewModel : ToolWindowBase, IProjectExplorerService
    {
        private readonly IHandleProjectExplorerItems[] handlers;

        public ProjectExplorerViewModel(
            IHandleProjectExplorerItems[] handlers)
        {
            DisplayName = "Explorer";

            this.handlers = handlers;

            Items = new ObservableCollection<ProjectExplorerItemBase>();

            SelectedItems = new ObservableCollection<ProjectExplorerItemBase>();
        }

        public ObservableCollection<ProjectExplorerItemBase> Items { get; set; }

        public ObservableCollection<ProjectExplorerItemBase> SelectedItems { get; set; }

        public bool IsSelected { get; set; }

        public Task AddItemAsync(ProjectExplorerItemBase item)
        {
            var handler = handlers.FirstOrDefault(h => h.CanHandle(item));

            if (handler != null)
            {
                item.PopulateContextMenu(handler);
            }

            if (string.IsNullOrWhiteSpace(item.Icon))
            {
                item.Icon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Circle.png";
            }

            if (item.ParentId != Guid.Empty)
            {
                var parent = this.Items.First(p => p.Id == item.ParentId);
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
            // :TODO: This needs to be recursive to delete all children
            var itemToDelete = Items.FirstOrDefault(i => i.Id == itemId);
            Items.Remove(itemToDelete);

            return Task.CompletedTask;
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
    }
}
