// <copyright file="ProjectExplorerItemBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Impulse.Shared.Interfaces;
using Impulse.Shared.ProjectExplorer;
using Impulse.SharedFramework.ProjectExplorer;
using ReactiveUI;

namespace Impulse.SharedFramework.Services.Layout;

public class ProjectExplorerItemBase : Caliburn.Micro.Screen, IHaveId
{
    public ProjectExplorerItemBase()
    {
        var type = this.GetType();
        Id = Guid.NewGuid();
        Items = new ObservableCollection<ProjectExplorerItemBase>();
        ContextMenu = new ContextMenu();

        ContextMenu.PreviewMouseUp += MouseUp;

        Callback = () => Task.CompletedTask;
        OriginalDisplayName = string.Empty;
    }

    public Guid Id { get; set; }

    public int Index { get; set; }

    public Guid ParentId { get; set; }

    public ObservableCollection<ProjectExplorerItemBase> Items { get; set; }

    public Func<Task> Callback { get; set; }

    public string Icon { get; set; }

    public bool IsExpanded { get; set; } = true;

    public bool IsEditable { get; set; } = true;

    public bool IsEditing { get; set; } = false;

    public bool IsEnabled { get; set; } = true;

    public bool IsVisible { get; set; } = true;

    public bool IsSelected { get; set; } = false;

    public ContextMenu ContextMenu { get; private set; }

    public string OriginalDisplayName { get; set; }

    internal void PopulateContextMenu(IHandleProjectExplorerItems handler)
    {
        if (handler == null)
        {
            return;
        }

        var items = handler.GetContextMenuItems(this);

        AddContextMenuItems(items);
    }

    internal void AddContextMenuItems(IEnumerable<ProjectExplorerContextMenuItem> menuItems)
    {
        if (menuItems == null)
        {
            return;
        }

        foreach (var menuItem in menuItems)
        {
            if (menuItem == null)
            {
                continue;
            }

            ContextMenu.Items.Add(CreateMenuItem(menuItem));
        }
    }

    private MenuItem CreateMenuItem(ProjectExplorerContextMenuItem menuItem)
    {
        var menuItemControl = new MenuItem
        {
            DataContext = menuItem,
            Header = menuItem.Title,
            Command = menuItem.Callback != null ? ReactiveCommand.CreateFromTask(menuItem.Callback) : null
        };

        if (menuItem.Image != null)
        {
            menuItemControl.Icon = new Image
            {
                Width = 16,
                Height = 16,
                Source = new BitmapImage(menuItem.Image)
            };
        }

        return menuItemControl;
    }

    private async void MouseUp(object sender, MouseButtonEventArgs e)
    {
        var menuItem = e.Source as MenuItem;
        if (menuItem?.DataContext is ProjectExplorerContextMenuItem contextMenuItem && menuItem.Command == null)
        {
            if (contextMenuItem.Callback != null)
            {
                await contextMenuItem.Callback();
            }
        }
    }
}
