// <copyright file="ProjectExplorerItemBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Impulse.Shared.Interfaces;
using Impulse.Shared.ProjectExplorer;
using Impulse.SharedFramework.ProjectExplorer;

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

        Callback = () => { };
    }

    public Guid Id { get; set; }

    public int Index { get; set; }

    public Guid ParentId { get; set; }

    public ObservableCollection<ProjectExplorerItemBase> Items { get; set; }

    public Action Callback { get; set; }

    public string Icon { get; set; }

    public bool IsExpanded { get; set; } = true;

    public bool IsEditable { get; set; } = true;

    public bool IsEditing { get; set; } = true;

    public bool IsEnabled { get; set; } = true;

    public bool IsVisible { get; set; } = true;

    public bool IsSelected { get; set; } = false;

    public ContextMenu ContextMenu { get; private set; }

    internal void PopulateContextMenu(IHandleProjectExplorerItems handler)
    {
        var items = handler.GetContextMenuItems(this);

        foreach (var item in items)
        {
            var grid = new Grid();
            grid.DataContext = item;
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto, MinWidth = 24 });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

            var image = new Image() { Width = 16, Height = 16 };

            if (item.Image != null)
            {
                image.Source = new BitmapImage(item.Image);
            }

            var text = new TextBlock() { Text = item.Title };

            Grid.SetColumn(grid, 0);
            grid.Children.Add(image);
            Grid.SetColumn(grid, 1);
            grid.Children.Add(text);

            ContextMenu.Items.Add(grid);
        }
    }

    private void MouseUp(object sender, MouseButtonEventArgs e)
    {
        var textBlock = e.Source as TextBlock;
        var grid = textBlock?.Parent as Grid ?? e.Source as Grid;
        var item = grid.DataContext as ProjectExplorerContextMenuItem;
        item.Callback();
    }
}
